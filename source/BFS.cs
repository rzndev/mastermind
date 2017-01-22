using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace MasterMind
{
    // класс, реализующий поиск решения в ширину
    public class BFS : MM_base
    {

        
        public BFS()
        {
            InitPatterns();
            stat = new Statistics();
        }


        /// <summary>
        /// Нахождение решения методом "в ширину"
        /// </summary>
        public void FindDecision_BFS()
        {
            int i;
            GameTreeNode u;

            ClearStat();
            stat.N = number_of_nodes;
            // определяем решение, к которому необходимо добраться методом "в ширину"
            byte[] pat = new byte[4];
            for (i = 0; i < 4; i++)
            {
                pat[i] = (byte)(board.GuessingRow[i] - 1);
            }
            board.bfs.secret_code = board.bfs.FindPattern(pat);
            bool bDecisionFound = false; // признак того, что решение найдено
            Queue<GameTreeNode> Q = new Queue<GameTreeNode>();
            
            Q.Enqueue(GameTree.children[0]); // обход начинаем с корневого элемента дерева решений
            stat.O++;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            int maxSize = 0;
            do
            {
                //u = Q.Peek();
                u = Q.Dequeue();
                stat.L++;
                
                if (u.item.pattern == secret_code)
                {
                    // искомое решение найдено - завершаем обход дерева
                    bDecisionFound = true;
                    break;
                }
                foreach (GameTreeNode n in u.children)
                {
                    stat.O++;
                    Q.Enqueue(n);
                }
                if (Q.Count > maxSize) maxSize = Q.Count;

            } while (Q.Count != 0);
            TimeSpan val = sw.Elapsed; // пройденное время
            stat.T = val.TotalSeconds;
            stat.V = maxSize * (sizeof(int) + sizeof(byte) + 30 * sizeof(int)); // 30 размер массива дочерних множеств
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
