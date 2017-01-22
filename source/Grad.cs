using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    public class Grad : MM_base
    {

        int foundedSolution;

        public Grad()
        {
            InitPatterns();
            stat = new Statistics();
        }

        /// <summary>
        /// Построение дерева решений методом градиентного спуска
        /// для достижения заданной комбинации code
        /// </summary>
        public GameTreeNode BuildTree(int code)
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
            number_of_nodes = 1;
            
            byte score = scores[code, root.item.pattern];
            BuildGameSet(root.item, initial.patterns, code);
            BuildTree(root, 1, code);
            GameTree = root;
            return root;
        }

        /// <summary>
        /// ветви дерева строим на основе оценки, полученной сравнением code с предполагаемой комбинацией
        /// построение прекращаем при достижении искомого решения
        /// </summary>
        /// <param name="node"></param>
        /// <param name="level"></param>
        public void BuildTree(GameTreeNode node, int level, int code)
        {
            int i, j;
            byte score;
            score = scores[code, node.item.pattern];
            if (score == NbrPegs * (NbrPegs - 1))
            { // решение найдено
                foundedSolution = node.item.pattern;
                return ;
            }
            stat.N++;
            stat.L++;
            stat.O++;
            Random r = new Random();
            if (level > 40)
            { // построение осуществляется только до определенного уровня
                return;
            }
            i = score;
            {
                TreeItemSet? item_set = node.item.childs[score];
                if (null == item_set) return; // ветка не ведет к решению
                HashSet<int> list_values = item_set.Value.patterns;
                // последующие ветви формируются на основе произвольно выбранной комбинации из list_values
                int size = list_values.Count;
                if (size == 0) return; // значений для перебор нет
                int num = r.Next(size); // получаем номер
                int value = list_values.ElementAt(num);
                //foreach (int value in list_values)
                {
                    // для каждого значения строим дерево исходов и добавляем его в потомки node
                    GameTreeNode vnode = new GameTreeNode();
                    vnode.children = new List<GameTreeNode>();
                    TreeItem set = new TreeItem();
                    set.pattern = value;
                    set.score = score; // стоимость перехода определяется положением в массиве node.item.childs
                    set.childs = new TreeItemSet?[30];
                    for (j = 0; j < 30; j++) set.childs[j] = null;
                    BuildGameSet(set, list_values, code);
                    // если количество потомков = 1 - решение найдено
                    if (set.childs[score].HasValue)
                    {
                        if (set.childs[score].Value.patterns.Count == 1)
                        {
                            foundedSolution = set.childs[score].Value.patterns.ElementAt(0); // решение найдено
                        }
                    }

                    vnode.item = set;
                    node.children.Add(vnode);
                    number_of_nodes++;
                    // строим дерево решений для потомков
                    BuildTree(vnode, level + 1, code);
                }
            }
        }


        /// <summary>
        /// Метод построения возможных исходов для данного элемента set с учетом code, имеющего тип TreeItem для рассматриваемых
        /// комбинаций patterns
        /// </summary>
        /// <param name="set"></param>
        public void BuildGameSet(TreeItem tree_item, HashSet<int> patterns, int code)
        {
            byte score;
            int val1 = tree_item.pattern;
            score = scores[code, val1]; // находим множество элементов, образующих данное количество очков
            foreach (int val2 in patterns) // находим соотношение каждой комбинации с каждой в множестве
            {
                if (score != scores[val1, val2]) continue;// CalcScore(val1, val2);
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
        /// поиск решения методом градиентного спуска
        /// </summary>
        public void FindDecision_Grad()
        {
            int i;
            ClearStat();
            // определяем решение, к которому необходимо добраться методом "в ширину"
            byte[] pat = new byte[4];
            for (i = 0; i < 4; i++)
            {
                pat[i] = (byte)(board.GuessingRow[i] - 1);
            }
            int secret_code = board.bfs.FindPattern(pat);

            bool bDecisionFound = false;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            BuildTree(secret_code);
            TimeSpan val = sw.Elapsed; // пройденное время
            stat.T = val.TotalSeconds;
            stat.V = (sizeof(int)+sizeof(byte)) * stat.O;
            bDecisionFound = foundedSolution == secret_code;

            if (bDecisionFound)
            {
                FormStatistics frm = new FormStatistics();
                frm.staticstics = stat;
                frm.Show();
            }
            else
            {
                MessageBox.Show("Решение не найдено");
            }

        }
    }
}
