using System;
using System.Collections.Generic;
using System.Text;

namespace MasterMind
{
    public class Statistics
    {
        public int O; // глубина поиска. максимальное число вершин. порожденных до целевой или тупиковой вершины в дереве вариантов. 
        public int L; // длина пути решения. число вершин лежащих на минимальном из найденных путей решений.   
        public int N; // общее число порожденных вершин
        //public double R; // разветвленность поиска. R=N/L.
        //public double W; // направленность поиска. W=N^(1/L).
        public double T; // время работы программы
        //public double tc; //эффективность просмотра вершин tc=T/N. где Т – время работы программы.
        public double V; // Емкость поиска. V = Vb*M, где Vb – объем оперативной памяти, занимаемой моделью ситуации в байтах.

        public override string ToString()
        {
            StringBuilder st = new StringBuilder();
            st.Append("Глубина поиска O: " + O).Append(Environment.NewLine);
            st.Append("Длина пути решения L: " + L).Append(Environment.NewLine);
            st.Append("Число порожденных вершин N: " + N).Append(Environment.NewLine);
            if (L != 0)
            {
                st.Append("Разветвленность поиска R: " + N / L).Append(Environment.NewLine);
                double W = Math.Pow(N, 1 / L);
                st.Append("Направленность поиска W: " + W).Append(Environment.NewLine);
            }
            else
            {
                st.Append("Разветвленность поиска R не определена, т.к. L=0").Append(Environment.NewLine);
                st.Append("Направленность поиска W не определена, т.к. L=0").Append(Environment.NewLine);

            }
            if (N != 0)
                st.Append("Эффективность просмотра вершин tc: " + T/N).Append(Environment.NewLine);
            else
                st.Append("Эффективность просмотра вершин tc не определена, т.к. N=0").Append(Environment.NewLine);
            st.Append("Емкость поиска V: " + V).Append(Environment.NewLine);
            return st.ToString();
        }
    }
}
