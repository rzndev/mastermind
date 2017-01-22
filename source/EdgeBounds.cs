using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterMind
{
    public class EdgeBounds : MM_base
    {
        public int CurGuessCount;

        /// <summary>
        /// Предполагаемая комбинация и результирующий ответ
        /// </summary>
        public struct TGuessRec
        {
            public int patternNbr;
            public int nbrinpos;
            public int nbroutofpos;
        }

        /// <summary>
        /// {количество допустимых коибинаций - используется для  подробного отчета}
        /// </summary>
        int TotOK;

        public TGuessRec[] CurGuesses;


        public struct MM_State
        {
            public TPatRec[] patterns;
            public TGuessRec[] CurGuesses;
            public int CurGuessCount;
        }


        public bool bFirstMove;

        public EdgeBounds()
        {
            InitPatterns();
            CurGuesses = new TGuessRec[30];
            CurGuessCount = 0;
            bFirstMove = true;

            stat = new Statistics();

        }

        /// <summary>
        /// Подсчет очков за выполненное предположение
        /// </summary>
        /// <param name="Masterpat"></param>
        public void Score(byte[] MasterPat, ref TGuessRec TestGuess)
        {
            int i, j;
            byte[] pat1, pat2;

            TestGuess.nbrinpos = 0;
            TestGuess.nbroutofpos = 0;
            pat1 = new byte[NbrPegs];
            pat2 = new byte[NbrPegs];
            patterns[TestGuess.patternNbr].Pattern.CopyTo(pat2, 0);
            MasterPat.CopyTo(pat1, 0);

            for (i = 0; i < NbrPegs; i++) if (pat2[i] == pat1[i]) TestGuess.nbrinpos++;
            for (i = 0; i < NbrPegs; i++)
                for (j = 0; j < NbrPegs; j++)
                    if ((pat2[i] == pat1[j]) && (pat2[i] != 255))
                    {
                        TestGuess.nbroutofpos++;
                        pat2[i] = 255; // в дальнейшем исключаем из поиска соответствий
                        pat1[j] = 255; // также исключаем из поиска соответствий
                    };
            TestGuess.nbroutofpos = TestGuess.nbroutofpos - TestGuess.nbrinpos;
        }

        /// <summary>
        /// Определение допустимости предположения
        /// основано на текущем предположении (guessrec), определяет может ли комбинация "testpatnbr" быть искомой комбинацией
        /// </summary>
        /// <returns></returns>
        bool Eligible(TGuessRec guessrec, int testpatnbr)
        {
            int i, j;
            int oldNbrRightPos, NewNbrRightPos;
            int oldNbrRightColor, NewNbrRightColor;
            byte[] Pat1;
            byte[] Pat2;
            TGuessRec TestGuess = new TGuessRec();
            bool result = true;
            oldNbrRightPos = guessrec.nbrinpos;
            oldNbrRightColor = oldNbrRightPos + guessrec.nbroutofpos;
            Pat1 = new byte[NbrPegs];
            Pat2 = new byte[NbrPegs];
            patterns[guessrec.patternNbr].Pattern.CopyTo(Pat1, 0); // текущее предположение
            patterns[testpatnbr].Pattern.CopyTo(Pat2, 0); // проверяем с этим шаблоном

            switch (Level)
            {
                case 1:
                    // самый простой
                    // эвристика
                    //#1  если общее количество соответсвует 
                    // 0 -   проверяемая комбинация не может содержать ни один из цветов в guessrec
                    if (result)
                    {
                        if (oldNbrRightColor == 0)
                            for (i = 0; i < NbrPegs; i++)
                                if (result)
                                {
                                    {
                                        j = 0;
                                        while ((j < NbrPegs) && result)
                                            if (Pat1[i] != Pat2[j])
                                            {
                                                j++;
                                            }
                                            else
                                            {
                                                result = false;
                                            };
                                    };
                                };
                    };


                    if (result)
                    //    #2 если все предположение - один цвет, номер этого цвета 
                    // в новом предположении должен точно соответствовать номеру в текущем предположении}
                    {
                        j = 1;
                        for (i = 1; i < NbrPegs; i++) if (Pat1[i] == Pat1[0]) j++;
                        if (j == NbrPegs)
                        {
                            j = 0;
                            for (i = 0; i < NbrPegs; i++) if (Pat2[i] == Pat1[1]) j++;
                            if (j != oldNbrRightColor)
                            {
                                result = false;
                            };
                        };
                    };

                    //#3 - Соответствие колышков в проверяемой комбинации при сравнении с предположением должно быть по крайней мере
                    //      настолько же высоким, как nbr в верной позиции предположения,
                    //      напр., предположение = RRGB и nbt в корректной позиции=1 а проверяемое кодовое слово = RGBV,
                    //      то сверяемое слово не допустимо, т.к. по крайней мере должно быть два соответсвия
                    //      предположению, ht только один колышек соответствует}
                    //      Так же если всего  в  или за пределами позиции = 2 , то количество в проверяемом кодовом слове
                    //      которое соответтсвует любой позиции в предположении, должно быть минимум 3,
                    //      например, предположение RRGB и сумма nbrв позиции и nbr на выходе
                    //       position = 2 тогда RVBB не допустима}
                    {
                        NewNbrRightPos = 0;
                        NewNbrRightColor = 0;
                        for (i = 0; i < NbrPegs; i++) if (Pat1[i] == Pat2[i]) ++NewNbrRightPos;
                        for (i = 0; i < NbrPegs; i++)
                        {
                            j = 0;
                            while ((j < NbrPegs) && (Pat1[i] != Pat2[j])) j++;

                            if ((j < NbrPegs) && (Pat1[i] == Pat2[j]))
                            {
                                NewNbrRightColor++;
                                Pat2[j] = 255; //{так не посчитаем его дважды при определении количества}
                            };
                        };
                        if (NewNbrRightPos > oldNbrRightPos) // != лучше для проверки, но делает слишком разумным!}
                        {
                            result = false;
                        };
                        if ((result) && (NewNbrRightColor < oldNbrRightColor))
                        {
                            result = false;
                        };
                    };
                    //{level=1}
                    break;
                case 2:
                case 3:
                    //Выбираем решение, которое имеет то же количество очков, что и предыдущее предположение.
                    //например, для игры с 4 колышками, каждое из 14 возможных очков будет частью 
                    //оставшихся комбинациях в 14 группах -- верное решение должно быть
                    //в группе, соответвующей предыдущему очку.  Удивительный результат
                    //основан на симметрии очков, score(секретный код, предполагаемый код)=
                    //score(предполагаемый код, секретный код) 
                    {
                        if (patterns[testpatnbr].OKFlag)
                        {
                            TestGuess.patternNbr = testpatnbr;
                            TestGuess.nbrinpos = -1;
                            TestGuess.nbroutofpos = -1;

                            Score(patterns[guessrec.patternNbr].Pattern, ref TestGuess);
                            if ((TestGuess.nbrinpos != guessrec.nbrinpos) ||
                                  (TestGuess.nbroutofpos != guessrec.nbroutofpos))
                                result = false;
                        }
                        else result = false;
                    }; //level=2
                    break;
            }// switch
            return result;
        }

        /// <summary>
        /// Сгенерировать предположительную комбинацию
        /// </summary>
        public int[] MakeGuess()
        {
            int i, j, k;
            int[] result = new int[NbrPegs];
            TGuessRec testguess = new TGuessRec();
            int minmax, minmaxguess, maxval;
            int[] maxscores = new int[30];
            int savecount;
            int nbrLeft;
            int[] TestPatternNbrs = new int[maxpatterns];
            int maxvalscore, minmaxscore;

            for (i = 0; i < NbrPegs; i++) result[i] = -1;

            if (bFirstMove == true)
            {
                bFirstMove = false;
                TotOK = maxpatterns;
                result = FirstGuess();
                stat.N++;
                stat.O++;
                return result;
            }

            {
                // устраняем из рассмотрения те комбинации, которые не возможны в текущем предположении

                savecount = TotOK;
                // убеждаемся, что текущий не выбран
                patterns[CurGuesses[CurGuessCount].patternNbr].OKFlag = false;
                TotOK--;
                // Помечаем все комбинации, которые не допустимы в текущем предположении
                for (i = 0; i < maxpatterns; i++)
                {
                    // Определение "допустимости" зависит от уровня
                    if (patterns[i].OKFlag && (!Eligible(CurGuesses[CurGuessCount], i)))
                    {
                        patterns[i].OKFlag = false;
                        TotOK--;
                    }
                };
            };
            stat.N += TotOK;
            //  {3. для уровней 1 или 2, выбираем первое допустимое значение
            //    в качестве следующего предположения
            if (Level < 3)
            {
                i = 0;
                while ((i < maxpatterns) &&
                (!patterns[i].OKFlag)) i++;
                if (!patterns[i].OKFlag)
                {
                    // сюда управление не должно попадать
                }
                else
                {
                    CurGuessCount++;
                    CurGuesses[CurGuessCount].patternNbr = i;
                    CurGuesses[CurGuessCount].nbrinpos = -1;
                    CurGuesses[CurGuessCount].nbroutofpos = -1;

                };
            }
            else// {for level 3 - use min-max technique}
            {
                // {Для каждого допустимого предположения получаем распределение очков, получаемое
                //  при проверке со всеми допустимыми значениями.  Сохраняем наибольшее для каждого предположения.
                //  Последовательность, используемая для следующих предположений, будет наименьшей из них}
                //
                // Для ускорения построим массив, содержащий только допустимые комбинации
                nbrLeft = 0;
                for (i = 0; i < maxpatterns; i++)
                {
                    if (patterns[i].OKFlag)
                    {
                        TestPatternNbrs[nbrLeft] = i;
                        nbrLeft++;
                    };
                };


                minmax = 9999;
                minmaxguess = 0;
                for (i = 0; i < nbrLeft; i++)  //для всех возможных комбинаций
                {
                    for (j = 0; j < maxscores.Length; j++) maxscores[j] = 0;
                    testguess.patternNbr = TestPatternNbrs[i];
                    // наиболее значимая часть. Находим, какому из 14 возможных наборов очков, 
                    // остающееся множество допустимых комбинаций соответствует
                    for (j = 0; j < nbrLeft; j++)
                    {
                        Score(patterns[TestPatternNbrs[j]].Pattern, ref testguess);
                        k = 5 * testguess.nbrinpos + testguess.nbroutofpos;
                        maxscores[k]++;
                    };
                    maxval = 0;
                    maxvalscore = 0;
                    // теперь находим наибольшее значение
                    for (j = 0; j < maxscores.Length; j++)
                        if (maxscores[j] > maxval)
                        {
                            maxval = maxscores[j];
                            maxvalscore = j; //для подробного отчета
                        };
                    if (maxval < minmax)
                    {
                        stat.O++;
                        minmaxguess = TestPatternNbrs[i];
                        minmax = maxval;
                        minmaxscore = maxvalscore;
                    };
                }; //for i
                // мы определили, какой образец содержит минимальное из максимальных чисел, которое и будет следующим предложением

                if (minmaxguess >= 0)
                {
                    CurGuessCount++;
                    CurGuesses[CurGuessCount].patternNbr = minmaxguess;
                    CurGuesses[CurGuessCount].nbrinpos = -1;
                    CurGuesses[CurGuessCount].nbroutofpos = -1;

                    for (i = 0; i < NbrPegs; i++)
                        result[i] = patterns[CurGuesses[CurGuessCount].patternNbr].Pattern[i] + 1;

                }
                else
                {
                    // сюда управление не должно передаваться
                };
            };
            return result;
        }

        public int[] FirstGuess()
        {
            int[] result = new int[NbrPegs];
            Random r = new Random();
            int i;
            TotOK = maxpatterns;
            switch (Level)
            {
                case 1:
                case 2:
                    CurGuesses[CurGuessCount].patternNbr = r.Next(maxpatterns);
                    break;
                case 3:
                    if (nbrpairs > 0)
                        CurGuesses[CurGuessCount].patternNbr = TwoPair[r.Next(nbrpairs)];
                    break;
            }
            for (i = 0; i < NbrPegs; i++)
                result[i] = patterns[CurGuesses[CurGuessCount].patternNbr].Pattern[i] + 1;
            return result;
        }

        /// <summary>
        /// Сохранить состояние решателя задачи
        /// </summary>
        /// <returns></returns>
        public MM_State SaveState()
        {
            MM_State result = new MM_State();

            result.CurGuesses = new TGuessRec[30];
            CurGuesses.CopyTo(result.CurGuesses, 0);

            result.CurGuessCount = CurGuessCount;

            result.patterns = new TPatRec[maxpatterns];
            patterns.CopyTo(result.patterns, 0);
            return result;
        }

        /// <summary>
        /// Восстановить состояние решателя задачи
        /// </summary>
        /// <param name="st"></param>
        public void RestoreState(MM_State st)
        {
            st.patterns.CopyTo(patterns, 0);
            st.CurGuesses.CopyTo(CurGuesses, 0);
            CurGuessCount = st.CurGuessCount;
        }


        /// <summary>
        /// Поиск решения с использованием стратегии ветвей и границ
        /// результат отображается на игровую доску
        /// </summary>
        public void SearchSolution()
        {
            int i;
            bFirstMove = true;
            CurGuessCount = 0;
            ClearStat();
            byte[] secret_code;
            secret_code = new byte[NbrPegs];
            for (i = 0; i < 4; i++)
            {
                secret_code[i] = (byte)(board.GuessingRow[i] - 1);
            }
            board.CurrentRow = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            stat.L = 0;
            sw.Start();

            do
            {
                // цикл выполняется пока не достигнуто решение
                int[] row = MakeGuess();
                Score(secret_code, ref CurGuesses[CurGuessCount]);
                foreach (int theColor in row)
                {
                    board.AddPeg(theColor, Board.destination.board);
                }
                board.CalcScore();
                board.AdvanceRow();
                stat.L++;

            } while (CurGuesses[CurGuessCount].nbrinpos != NbrPegs);
            TimeSpan val = sw.Elapsed; // пройденное время
            stat.V = stat.N * patterns.GetLength(0);
            stat.T = val.TotalSeconds;
            FormStatistics frm = new FormStatistics();
            frm.staticstics = stat;
            frm.Show();
        }
        
    }
}
