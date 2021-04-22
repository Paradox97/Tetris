using System;
using System.Linq;
using System.Drawing;
using System.Timers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using System.Collections.Generic;


namespace Tetris
{
    public class Field
    {
        public int
            width, height,
            block_size,
            start_x, start_y,
            score, period;

        public bool
            gameover,
            exit, pause;

        public char[,] area;

        string player_name;

        public struct text_menu
        {
            public string order;
            public string message;

            public text_menu(string order, string message)
            {
                this.order = order;
                this.message = message;
            }

        }

        public List<text_menu> text_menus;

        public Field(int width, int height, int start_x, int start_y, int block_size, int difficulty, string player_name) //user defined field constructor 
        {
            this.height = height;
            this.width = width;
            this.start_x = start_x;
            this.start_y = start_y;
            this.block_size = block_size;
            this.player_name = player_name;
            this.period = 150;  //from constructor

            switch (difficulty)
            {
                case 1:
                    this.period = 150;
                    break;
                case 2:
                    this.period = 120;
                    break;
                case 3:
                    this.period = 90;
                    break;
            }

            area = new char[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    area[i, j] = ' ';
                }
            }
        }

        public void render()
        {
            Console.SetCursorPosition(0, 0);

            string output = string.Empty;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output += area[j, i];
                }
                output += "|\n";

                if (i == 1)
                {
                    for (int j = 0; j < width; j++)
                    {
                        output += '-';
                    }
                    output += "|\n";
                }
            }

            for (int j = 0; j < width; j++)
            {
                output += "^";
            }

            output += "|\n Score: " + score + "\n (Space)Rotate" + " (<-)Left " + "(->)Right (↓)Down \n (P)Pause (F)Faster (S)Slower (Q)Quit";

            Console.Write(output);

        }

        public int update(ConsoleKeyInfo input, int PLAYER_TIMEOUT, Field grid, Shape figure)
        {
            input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.LeftArrow:
                    figure.move_left(grid);
                    break;              //watch breaks and returns

                case ConsoleKey.RightArrow:
                    figure.move_right(grid);
                    break;

                case ConsoleKey.DownArrow:
                    figure.move_down(grid);
                    break;

                case ConsoleKey.Spacebar:
                    figure.rotate(grid);
                    break;

                case ConsoleKey.Q:
                    grid.exit = true;
                    return 1;

                case ConsoleKey.P:
                    grid.pause = !grid.pause;
                    return 2;

                case ConsoleKey.F:
                    if (grid.period > 10)
                        grid.period -= 10;
                    return 3;

                case ConsoleKey.S:
                    if (grid.period < 300)
                        grid.period += 10;
                    return 4;
            }

            return 0;
        }

        static void high_score()
        {
            string file_name = "Tetris.records";
            string path = AppDomain.CurrentDomain.BaseDirectory + file_name;
            string output = string.Empty;
            if (!File.Exists(path))
            {
                using (StreamWriter write = File.CreateText(path))
                {
                    for (int i = 0; i < 67; i++)
                    {
                        output += "#";
                    }
                    write.WriteLine(output);
                }

            }
        }

        public void game_over()
        {
            Console.WriteLine("12312312341441q");
            System.Threading.Thread.Sleep(500);

        }

        static int navigation(ConsoleKeyInfo input)
        {
            switch (input.Key)
            {
                case ConsoleKey.S:
                    return 0;

                case ConsoleKey.H:
                    return 1;

                case ConsoleKey.Q:
                    return 2;

                case ConsoleKey.Y:
                    return 3;

                case ConsoleKey.N:
                    return 4;

                case ConsoleKey.D1:
                    return 5;

                case ConsoleKey.D2:
                    return 6;

                case ConsoleKey.D3:
                    return 7;
            }
            return 8;

        }

        static void start(string[] alltext, int state)
        {
            string output = string.Empty;
            ConsoleKeyInfo input_key;

            int choice_start_menu = 0;

            Console.SetCursorPosition(0, 0);
            output = alltext[0] + alltext[7];
            //Console


        }

        static int static_screens_travel(string[] alltext, int state, int choice)
        {
            switch (state) 
            {
                case 0:             //main menu
                    switch (choice)
                    {
                        case 0:
                            return 1;
                        case 1:
                            return 1;
                        case 2:
                            escape(alltext, state);
                            return 2;
                        case 3:
                            return 3;
                        case 4:
                            return 4;
                    }
                 break;
                case 1:            //difficulty choice menu
                    break;
                case 2:            //field size choice menu
                    break;
                case 4:            //high score screen
                    break;
                case 5:            //quit screen
                    break;  
            }
            return 5;
        }

        static void main_menu(string[] alltext, int state)
        {
            string output = string.Empty;

            int choice_main_menu = 0;
            ConsoleKeyInfo input_key;

            Console.SetCursorPosition(0, 0);

            output = alltext[0] + alltext[2];

            while (alltext[10] == string.Empty)
            {
                Console.Write(output);
                alltext[10] = Console.ReadLine();
                Console.Clear();
            }
            Console.SetCursorPosition(0, 0);
            output = alltext[0] + alltext[3] + alltext[10] + alltext[4] + alltext[5];
            Console.Write(output);

            input_key = Console.ReadKey(true);
            Console.Clear();

            while ((choice_main_menu = navigation(input_key)) > 7)
            {
                Console.Write(output);
                input_key = Console.ReadKey(true);
                Console.Clear();
            }


            switch (choice_main_menu)
            {
                case 0:
                    game(15, 20, alltext, state);
                    break;
                case 1:
                    high_score();
                    break;
                case 2:
                    escape(alltext, state);
                    return;

                case 3:
                    return;

                case 4:
                    return;
            }




        }

        static void escape(string[] alltext, int state)
        {
            int choice = 0;
            ConsoleKeyInfo input_key;
            Console.Write("Exit menu");
            input_key = Console.ReadKey(true);
            Console.Clear();

            while ((choice = navigation(input_key)) > 7)
            {
                Console.Write("Exit menu");
                input_key = Console.ReadKey(true);
                Console.Clear();
            }

            switch (choice)
            {
                case 3:
                    return;
                case 4:
                    main_menu(alltext, state);
                    break;
            }
        }

        static int game(int width, int height, string[] alltext, int state)
        {
            Console.WriteLine("Press Any key");
            const int PLAYER_TIMEOUT = 3;

            int counter = 0;

            Field grid = new Field(width, height, 0, 0, 10, 0, alltext[10]);


            Shape figure = new Shape(grid);

            ConsoleKeyInfo input = Console.ReadKey(true);

            grid.render();

            while (grid.exit == false)
            {
                if (grid.gameover == true)
                    return 4;   //game over state

                grid.render();

                Task<int> task_update = new Task<int>(() => grid.update(input, PLAYER_TIMEOUT, grid, figure));
                task_update.Start();

                while (grid.pause == true)
                {
                    if (grid.exit)
                        return 3;       //quit
                    System.Threading.Thread.Sleep(500);
                }

                if ((counter % grid.period) == 0)
                {
                    figure.move_down(grid);  //giving time to react
                }

                counter++;
                System.Threading.Thread.Sleep(PLAYER_TIMEOUT);
            }

            return 1; //game over
        }

        static void Main(string[] args)
        {
            //all text menu divided by pieces
            string[] alltext =
                { "##########W E L C O M E         T O         T E T R I S !##########", //0
                "\n###########H I G H                           S C O R E S###########", //1
                "\nEnter player name: ", //2 
                "\nPlayer: ", //3
                "\n(H)Show highscores ", //4 
                "\n(Q)Quit Tetris ", //5
                "\nAre you sure(Y/N)?", //6
                "\nChoose difficulty:\n(1)Easy\n(2)Medium\n(3)Hard,", //7
                "\nChoose field size:\n(1)Little\n(2)Medium\n(3)Big", //8
                "\nPlayer                                                        Score", //9
                "", //10
                "\n(Backspace)Back", //11
                "\n(Esc)Back to the main menu" //12
                };
            int state = 0;
            

            //main menu - 1) 0 2    2)0 3 10 4 5
            //start menu - 1) 0 7 11   2)0 8 11
            //quit menu - 1) 0 6
            //high scores 1) 0 1 9 %highscores%

            main_menu(alltext, state);
            //high_score();
            //string player_name = string.Empty;
            //game(15,20, player_name);
            return;
        }

    }

}