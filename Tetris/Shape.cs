using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;


namespace Tetris
{
    public class Shape
    {
        //public int x_coord, y_coord;
        const int SHAPE_QUANTITY = 5, SHAPE_SIZE = 4;

        public enum Shapetype
        {
            line = 0,
            l_shape = 1,
            square = 2,
            t_shape = 3,
            s_shape = 4
        }

        int centre, shape_type;

        public struct shape_block
        {
            public int x_coord;
            public int y_coord;
            
            public shape_block(int x, int y)
            {
                this.x_coord = x;
                this.y_coord = y;
            }

        }

        public List<shape_block> shape_map;

        public Shape(int X, int Y) //Shape constructor
        {
            //current_shape = SHAPES[shape_type];
            shape_map_create(X, Y);
        }

        public void shape_map_create(int X, int Y)      //hard coded shapes
        {
            Random rand = new Random();
            
            shape_type = rand.Next(0, 4);

            shape_map = new List<shape_block>();

            switch (shape_type)
            {
                case 0://line shape

                    for (int i = 0; i < 4; i++)
                    {
                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + i,
                            y_coord = Y
                        }
                        );
                    }

                    centre = 1;

                    break;

                case 1://l shape

                    shape_map.Add(new shape_block()
                    {
                        x_coord = X,
                        y_coord = Y
                    }
                    );

                    
                    for (int i = 0; i < 3; i++)
                    {
                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + i,
                            y_coord = Y + 1
                        }
                        );
                    }

                    centre = 2;
                    
                    break;

                case 2://square shape

                    for (int i = 0; i < 2; i++)
                        shape_map.Add(new shape_block()
                    {
                        x_coord = X + i,
                        y_coord = Y
                    }
                   );

                    for (int i = 0; i < 2; i++)
                    {
                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + i,
                            y_coord = Y + 1
                        }
                        );
                    }

                    centre = 0; 

                    break;

                case 3://t shape

                    shape_map.Add(new shape_block()
                    {
                        x_coord = X + 1,
                        y_coord = Y
                    }
                   );

                    for (int i = 0; i < 3; i++)
                    {
                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + i,
                            y_coord = Y + 1
                        }
                        );
                    }

                    centre = 2;

                    break;

                case 4: //s shape


                    for (int i = 0; i < 2; i++)
                    {
                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + i,
                            y_coord = Y
                        }
                        );
                    }

                    for (int i = 0; i < 2; i++)
                    {
                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + 1 + i,
                            y_coord = Y + 1
                        }
                        );
                    }

                    centre = 3;

                    break;
            }

        }


        public int rotate(Field grid)
        {

            if (shape_type == 2)
                return 1;

            List<shape_block> rotation_map = new List<shape_block>();

            //placing shape block at (0,0) for rotation simplicity then place it back where it was, delta = current coordinates of the figure

            //создавать при создании фигуры центр
            
            int delta_x = shape_map[centre].x_coord;
            int delta_y = shape_map[centre].y_coord;
            


            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                rotation_map.Add(new shape_block { x_coord = shape_map[i].x_coord - delta_x, y_coord = shape_map[i].y_coord - delta_y});
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                //  rotation_map[i] = new shape_block { x_coord = rotation_map[i].y_coord + delta_x, y_coord = SHAPE_SIZE - shape_map[i].x_coord + delta_y};
                rotation_map[i] = new shape_block { x_coord = rotation_map[i].y_coord, y_coord = SHAPE_SIZE - shape_map[i].x_coord};
            }


            //смещение на такое количество клеток, чтобы центр оказался в той же точке, что и был
            int delta_new_x = Math.Abs(rotation_map[centre].x_coord - delta_x);

            int delta_new_y = Math.Abs(rotation_map[centre].y_coord - delta_y);


            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                //  rotation_map[i] = new shape_block { x_coord = rotation_map[i].y_coord + delta_x, y_coord = SHAPE_SIZE - shape_map[i].x_coord + delta_y};
                rotation_map[i] = new shape_block { x_coord = rotation_map[i].x_coord + delta_new_x, y_coord = rotation_map[i].y_coord + delta_new_y };
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                if ((rotation_map[i].x_coord > grid.width - 1) || (rotation_map[i].y_coord > grid.height - 1) || (rotation_map[i].x_coord < 0) || (rotation_map[i].y_coord < 0))
                {
                    return 1;
                }
            }

            /*
             for (int i = 0; i <rotation_map.Count; i++)           //checking collisions with near blocks
             {
                 if (grid.area[rotation_map[i].x_coord, rotation_map[i].y_coord] == 1)
                 {
                     return 1;
                 }
             }*/


            /* for (int i = 0; i < shape_map.Count; i++)
             {
                 shape_map[i] = new shape_block { x_coord = rotation_map[i].x_coord, y_coord = rotation_map[i].y_coord };
                 grid.area[rotation_map[i].x_coord, rotation_map[i].y_coord] = '*';
             }
            */


            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                grid.area[shape_map[i].x_coord, shape_map[i].y_coord] = ' ';      //cleaning up figure trails  
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                shape_map[i] = new shape_block { x_coord = rotation_map[i].x_coord, y_coord = rotation_map[i].y_coord };
                grid.area[rotation_map[i].x_coord, rotation_map[i].y_coord] = '#';
            }

            return 0;

        }

        public int move_down(Field grid)
        {
            List<shape_block> move_down_map = new List<shape_block>();

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                move_down_map.Add(new shape_block { x_coord = shape_map[i].x_coord, y_coord = shape_map[i].y_coord + 1});
                //Console.WriteLine(move_down_map[i].y_coord);
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                if ((move_down_map[i].x_coord > grid.width - 1) || (move_down_map[i].y_coord > grid.height - 1) || (move_down_map[i].x_coord < 0) || (move_down_map[i].y_coord < 0))
                {
                    shape_map_create(grid.start_x, grid.start_y);


                    return 1;
                }
                //Console.WriteLine(move_down_map[i].y_coord);
            }

            /*
            for (int i = 0; i < move_down_map.Count; i++)           //checking collisions with near blocks
            {
                if (grid.area[move_down_map[i].x_coord, move_down_map[i].y_coord] == 1)             //if the block is at the "bottom" already
                {
                    //Console.WriteLine("3");
                    stop_falling = true;
                }
            }*/

            for (int i = 0; i < SHAPE_SIZE; i++)
              {
                  grid.area[shape_map[i].x_coord, shape_map[i].y_coord] = ' ';              //cleaning up figure trails  
              }

            for (int i = 0; i < SHAPE_SIZE; i++)
              {
                  shape_map[i] = new shape_block { x_coord = move_down_map[i].x_coord, y_coord = move_down_map[i].y_coord };
                  grid.area[move_down_map[i].x_coord, move_down_map[i].y_coord] = '#';
              }
         
            //stop_falling = false;

            return 0;
        }


        public int move_right(Field grid)
        {
            List<shape_block> move_right_map = new List<shape_block>();

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                move_right_map.Add(new shape_block { x_coord = shape_map[i].x_coord + 1, y_coord = shape_map[i].y_coord });
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                if ((move_right_map[i].x_coord > grid.width - 1) || (move_right_map[i].y_coord > grid.height - 1))
                {
                    return 1;
                }
                //Console.WriteLine(move_down_map[i].y_coord);
            }

            /* for (int i = 0; i < move_right_map.Count; i++)           //checking collisions with near blocks
             {
                 if (grid.area[move_right_map[i].x_coord, move_right_map[i].y_coord] == 1)
                     return 1;
             }
            */

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                grid.area[shape_map[i].x_coord, shape_map[i].y_coord] = ' ';      //cleaning up figure trails  
            }
            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                shape_map[i] = new shape_block { x_coord = move_right_map[i].x_coord, y_coord = move_right_map[i].y_coord };
                grid.area[move_right_map[i].x_coord, move_right_map[i].y_coord] = '#';
            }

            return 0;
            //x_coord++;
        }

        public int move_left(Field grid)
        {
            List<shape_block> move_left_map = new List<shape_block>();

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                move_left_map.Add(new shape_block { x_coord = shape_map[i].x_coord - 1, y_coord = shape_map[i].y_coord });
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                if ((move_left_map[i].x_coord > grid.width - 1) || (move_left_map[i].y_coord > grid.height - 1) || (move_left_map[i].x_coord < 0) || (move_left_map[i].y_coord < 0))
                {
                    return 1;
                }
                //Console.WriteLine(move_down_map[i].y_coord);
            }

            /*
             for (int i = 0; i < move_left_map.Count; i++)           //checking collisions with near blocks
             {
                 if (grid.area[move_left_map[i].x_coord, move_left_map[i].y_coord] == 1)
                     return 1;
             }
            */

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                grid.area[shape_map[i].x_coord, shape_map[i].y_coord] = ' ';
            }
            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                shape_map[i] = new shape_block { x_coord = move_left_map[i].x_coord, y_coord = move_left_map[i].y_coord };
                grid.area[move_left_map[i].x_coord, move_left_map[i].y_coord] = '#';
            }

            return 0;
            //x_coord--;
        }


    }
}