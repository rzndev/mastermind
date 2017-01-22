using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MasterMind
{
    // базовый класс для mastermind
    public class MM_base
    {
        public const int NbrColors = 8; // количество используемых цветов
        public const int NbrPegs = 4;   // количество используемых колышков
        public const int maxpatterns = 4096; // количество кодовых комбинаций 8^4 = 4096
        public int Level = 3;               //Уровень сложности: 1=новичек, 2=хороший, 3=непобедимый
        public Board board;              // используется для управления содержимым игрового поля

        public Statistics stat;

        public void ClearStat()
        {
            stat.O = 0;
            stat.L = 0;
            stat.N = 0;

            stat.V = 0;

            stat.T = 0;
        }

        /// <summary>
        /// Информация о кодовом слове.
        /// </summary>
        public struct TPatRec
        {
            public byte[] Pattern;
            public bool OKFlag;
        }

        public TPatRec[] patterns; // все допустимые кодовые комбинации
        public int[] TwoPair;
        public int nbrpairs;


        /// <summary>
        /// заполнить все возможные кодовые комбинации
        public void InitPatterns()
        {
            //byte i, j;
            byte k, l, m, n;
            int count;
            patterns = new TPatRec[maxpatterns];
            TwoPair = new int[maxpatterns];
            
            count=0;
            nbrpairs=0;
//            for (i= 0; i < NbrColors; i++)
//                for (j=0; j < NbrColors; j++)
                    for (k=0; k < NbrColors; k++)
                        for (l=0; l < NbrColors; l++)
                            for (m=0; m < NbrColors; m++)
                                for(n = 0; n < NbrColors; n++)
                                {
                                    patterns[count].Pattern = new byte[NbrPegs];
                                    patterns[count].OKFlag = true;
                                    //if (NbrPegs>=6) patterns[count].Pattern[NbrPegs-6]=i;
                                    //if (NbrPegs>=5) patterns[count].Pattern[NbrPegs-5]=j;
                                    if (NbrPegs >= 4)
                                    {
                                        patterns[count].Pattern[NbrPegs - 4] = k;
                                    }
                                    patterns[count].Pattern[NbrPegs-3]=l;
                                    patterns[count].Pattern[NbrPegs-2]=m;
                                    patterns[count].Pattern[NbrPegs-1]=n;

                                    //if (((i==j) && (k==l) && (i!=k))
                                    //|| ((i==k) && (j==l) && (i!=j))
                                    //|| ((i==l) && (j==k) && (i!=j)))

                                    if (((k==l) && (m==n) && (k != m)) ||
                                       ((k==m) && (l==n) && (k != l)) ||
                                       ((k==n) && (l==m) && (k!= l)))
                                    {
                                      nbrpairs++;
                                      TwoPair[nbrpairs]=count;
                                    }
                                    count++;
                                    }
        }



        public bool bFoundSolution;

        public byte[,] scores;

        byte CalcScore(int v1, int v2)
        {
            byte result = 0;
            int i, j;
            byte[] pat1, pat2;
            int nbrinpos = 0;
            int nbroutofpos = 0;
            
            pat1 = new byte[NbrPegs];
            pat2 = new byte[NbrPegs];
            patterns[v1].Pattern.CopyTo(pat2, 0);
            patterns[v2].Pattern.CopyTo(pat1, 0);

            for (i = 0; i < NbrPegs; i++) if (pat2[i] == pat1[i]) nbrinpos++;
            for (i = 0; i < NbrPegs; i++)
                for (j = 0; j < NbrPegs; j++)
                    if ((pat2[i] == pat1[j]) && (pat2[i] != 255))
                    {
                        nbroutofpos++;
                        pat2[i] = 255; // в дальнейшем исключаем из поиска соответствий
                        pat1[j] = 255; // также исключаем из поиска соответствий
                    };
            nbroutofpos = nbroutofpos - nbrinpos;
            result = (byte)((NbrPegs+1) * nbrinpos + nbroutofpos);
            return result;
        }

        /// <summary>
        /// Построение таблицы расстояний между оценками. Используется для ускорения работы алгоритма построения
        /// дерева решений
        /// </summary>
        /// <returns></returns>
        public byte[,] BuildScoresTable()
        {
            int i, j;
            byte[,] scores = new byte[maxpatterns,maxpatterns];
            for(i = 0; i < maxpatterns; i++)
                for (j = i; j < maxpatterns; j++)
                {
                    scores[i, j] = CalcScore(i, j);
                    scores[j, i] = scores[i, j];
                }
            this.scores = scores;
            return scores;
        }

        /// <summary>
        /// Набор предполягаемых решений
        /// </summary>
        public struct TreeItemSet
        {
            public HashSet<int> patterns; // допустимые к рассмотрению комбинации
        }
        /// <summary>
        /// элемент решения
        /// </summary>
        public struct TreeItem
        {
            public int pattern;       // рассматриваемая комбинация.
            public byte score;        // количество очков, при которой произошел переход от предыдущей комбинации к текущей комбинации
            public TreeItemSet?[] childs; // потомки, определяющие определенное количество очков с рассматриваемой комбинацией.
                                      // childs используются для построения дерева решения
        }

        /// <summary>
        /// Дерево, по которому ищется решение игры
        /// </summary>
        public struct GameTreeNode
        {
            public List<GameTreeNode> children;  // список потомков
            public TreeItem item;                       // вершина дерева, представляющая комбинацию колышков
        }


        public int number_of_nodes; // количество узлов игрового дерева

        public GameTreeNode GameTree; // дерево решений
        public int secret_code;       // секретный код, который должен быть обнаружен алгоритмом


        /// <summary>
        /// Построение полного дерева решений игры
        /// </summary>
        public GameTreeNode BuildTree()
        {
            int i;
            GameTreeNode root = new GameTreeNode();
            TreeItemSet initial = new TreeItemSet();
            TreeItem root_treeitem = new TreeItem();
            root_treeitem.pattern = 0;
            root_treeitem.childs = new TreeItemSet?[30];
            for (i = 0; i < 30; i++) root_treeitem.childs[i] = null;
            initial.patterns = new HashSet<int>();
            for (i = 0; i < maxpatterns; i++) initial.patterns.Add(i);
            root_treeitem.childs[0] = initial;
            root_treeitem.score = 0;
            root.item = root_treeitem;
            root.children = new List<GameTreeNode>();
            number_of_nodes=1;
            BuildTree(root, 1);
            GameTree = root;
            return root;
        }

        /// <summary>
        /// метод построения ветвей дерева решения
        /// </summary>
        /// <param name="node"></param>
        /// <param name="level"></param>
        public void BuildTree(GameTreeNode node, int level)
        {
            int i,j;
            Random r = new Random();
            if (level > 10) return; // построение осуществляется только до определенного уровня
            for(i = 0; i < 30; i++)
            {
                TreeItemSet? item_set = node.item.childs[i];
                if (null == item_set) continue;
                HashSet<int> list_values = item_set.Value.patterns;
                // последующие ветви формируются на основе произвольно выбранной комбинации из list_values
                int size = list_values.Count;
                if (size == 0) continue;
                int num = r.Next(size); // получаем номер
                int value = list_values.ElementAt(num);
                //foreach (int value in list_values)
                {
                    // для каждого значения строим дерево исходов и добавляем его в потомки node
                    GameTreeNode vnode = new GameTreeNode();
                    vnode.children = new List<GameTreeNode>();
                    TreeItem set = new TreeItem();
                    set.pattern = value;
                    set.score = (byte)i; // стоимость перехода определяется положением в массиве node.item.childs
                    set.childs = new TreeItemSet?[30];
                    for (j = 0; j < 30; j++) set.childs[j] = null;
                    BuildGameSet(set, list_values);
                    vnode.item = set;
                    node.children.Add(vnode);
                    number_of_nodes++;
                    // строим дерево решений для потомков
                    BuildTree(vnode, level + 1);
                }
            }
        }

        /// <summary>
        /// Метод построения возможных исходов для данного элемента set, имеющего тип TreeItem для рассматриваемых
        /// комбинаций patterns
        /// </summary>
        /// <param name="set"></param>
        public void BuildGameSet(TreeItem tree_item, HashSet<int> patterns)
        {
            byte score;
            int val1 = tree_item.pattern;
            foreach (int val2 in patterns) // находим соотношение каждой комбинации с каждой в множестве
            {
                score = scores[val1, val2];// CalcScore(val1, val2);
                if (null == tree_item.childs[score])
                {
                    TreeItemSet itemSet = new TreeItemSet();
                    itemSet.patterns = new HashSet<int>();
                    tree_item.childs[score] = itemSet;
                }
                TreeItemSet item = tree_item.childs[score].Value;
                item.patterns.Add(val2);
            }
            
            
        }

        /// <summary>
        /// Поиск комбинации в массиве комбинаций
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public int FindPattern(byte[] code)
        {
            int result = 0;
            int i,j;
            byte[] pat1;
            byte[] pat2;

            pat1 = new byte[NbrPegs];
            pat2 = new byte[NbrPegs];

            code.CopyTo(pat2, 0);

            int nbrinpos;

            for (i = 0; i < maxpatterns; i++)
            {
                patterns[i].Pattern.CopyTo(pat1, 0);
                nbrinpos = 0;
                for (j = 0; j < NbrPegs; j++) if (pat2[j] == pat1[j]) nbrinpos++;
                if (nbrinpos == NbrPegs)
                {
                    result = i;
                    break;
                }
            }
            
            return result;
        }

    }
}
