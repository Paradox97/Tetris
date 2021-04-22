using System;
using System.Linq;
using System.Drawing;
using System.Timers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;


namespace Tetris
{
    public class Field
    {
        public int width, height;

        public int block_size;

        public int score = 0;

        public int start_x, start_y;

        public bool gameover = false;

        public char[,] area;

        string player_name;

        bool exit = false, pause = false;

        int period = 150;


        public Field(int width_rec, int height_rec, int startx, int starty, int blocksize, string player_name_rec) //user defined field constructor 
        {
            height = height_rec;
            width = width_rec;
            start_x = startx;
            start_y = starty;
            block_size = blocksize;
            player_name = player_name_rec;


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

                if(i == 1)
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

            output += "|\n Score: " + score + "\n (Space)Rotate"+ " (<-)Left " + "(->)Right (↓)Down \n (P)Pause (F)Faster (S)Slower (Esc)Exit";

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

                    case ConsoleKey.Escape:
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

                case ConsoleKey.Escape:
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

        static void start()
        {


        }

        static void main_menu()
        {
            //menu divided by pieces
            string greetings, player_name_req = string.Empty;
            string start = string.Empty;
            string player_name_show, player_name = string.Empty; 
            string quit, quit_sure = string.Empty;
            string highscore_menu, greetings_highscore, highscore = string.Empty;
            string output,input = string.Empty;

            int choice_main_menu = 0;
            ConsoleKeyInfo input_key;

            Console.SetCursorPosition(0, 0);
            greetings = "##########W E L C O M E         T O         T E T R I S !##########";
            greetings_highscore = "Highscores";
            player_name_req = "\nEnter player name: ";
            player_name_show = "\nPlayer: "; 
            highscore_menu = "\n(H)Show highscores";
            quit = "\n(Esc)Quit Tetris";
            quit_sure = "\nAre you sure(Y/N)?";

            output = greetings + player_name_req;

            while(player_name == string.Empty)
            {
                Console.Write(output);
                player_name = Console.ReadLine();
                Console.Clear();
            }
            Console.SetCursorPosition(0, 0);
            output = greetings + player_name_show + player_name + highscore_menu + quit;
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
                    game(15, 20, player_name);
                    break;
                case 1:
                    high_score();
                    break;
                case 2:
                    escape();
                    return;

                case 3:
                    return;

                case 4:
                    return;
            }




        }

        static void escape()
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
                    main_menu();
                    break;
            }
        }

        static int game(int width, int height, string player_name)
        {
            Console.WriteLine("Press Any key");
            const int PLAYER_TIMEOUT = 3;

            int counter = 0;

            Field grid = new Field(width, height, 0, 0, 10, player_name);


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
            main_menu();
            //high_score();
            //string player_name = string.Empty;
            //game(15,20, player_name);
            return;
        }

    }

}