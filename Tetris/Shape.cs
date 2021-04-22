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

        private enum Shapetype
        {
            line = 0,
            l_shape = 1,
            square = 2,
            t_shape = 3,
            s_shape = 4
        }

        int center, shape_type;

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

        public Shape(Field grid) //Shape constructor
        {
           shape_map_create(grid);
        }

        private int shape_map_create(Field grid)      //hard coded shapes
        {
            int X = grid.start_x;
            int Y = grid.start_y;
                //FIX RANDOM
            Random rand = new Random();

            shape_type = rand.Next(0, 5);

            shape_map = new List<shape_block>();
            List<shape_block> check_map = new List<shape_block>();

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

                        center = 1;
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

                        center = 2;
                        break;
                        
                    case 2: //reverse l shape

                        shape_map.Add(new shape_block()
                        {
                            x_coord = X + 2,
                            y_coord = Y
                        }
                        );


                        for (int i = 0; i < 3; i++)
                        {
                            shape_map.Add(new shape_block()
                            {
                                x_coord = (X + 2) - i,
                                y_coord = Y + 1
                            }
                            );
                        }

                        center = 2;
                        break;

                    case 3://square shape

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

                        center = 0;
                        break;

                    case 4://t shape

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

                        center = 2;
                        break;

                    case 5: //s shape


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

                        center = 3;
                        break;
                }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                check_map.Add(new shape_block { x_coord = shape_map[i].x_coord, y_coord = shape_map[i].y_coord + 1 });
            }

            int result = check_collisions(check_map, shape_map, grid);

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                grid.area[shape_map[i].x_coord, shape_map[i].y_coord] = '#';
            }


            if (result == 1)
            {
                return 1;
            }
            else 
                return 0;

        }


        public void clear_row(Field grid, int row)
        {
            for (int j = 0; j < grid.width; j++)
            {
                grid.area[j, row] = ' ';
                grid.score += 10;
            }
            apply_gravity(grid, row);
        }

        public void apply_gravity(Field grid, int row)
        {

            if (row - 1 < 0)
            {
                return;
            }

            for (int i = row; i > 0; i--)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    grid.area[j, i] = grid.area[j, i - 1];
                    //grid.area[j, i - 1] = ' ';  // только если гравитация действует на 1 ряд
                }
            }


        }

        public void check_blast(Field grid)
        {
            int row = 0;
            string row_map = String.Empty;

            for (int i = 0; i < grid.height; i++)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    row_map += grid.area[j, i];
                }

                row = i;

                if (row_map.IndexOf(' ') == -1)
                {
                    clear_row(grid, row);
                }

                row_map = String.Empty;
            }
        }

        private int check_collisions(List<shape_block> change_map, List<shape_block> orig_map, Field grid)
        {
            
              for (int i = 0; i < change_map.Count; i++)           
              {
                  if ((grid.area[change_map[i].x_coord, change_map[i].y_coord] == '#') && (check_self(change_map[i], orig_map) == 1) ) //check if self
                  {
                      return 1;
                  }
              }

              return 0;
        }


        private int check_self(shape_block change_map_part, List<shape_block> orig_map)  //if piece of the new map corresponds to original map
        {
            for (int i = 0; i < orig_map.Count; i++)
            {
                if ((orig_map[i].x_coord == change_map_part.x_coord) && (orig_map[i].y_coord == change_map_part.y_coord))
                {
                    return 0;
                }
            }
            return 1;
        }


        private int check_out_of_bounds(List<shape_block> map, Field grid)
        {
            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                if ((map[i].x_coord > grid.width - 1) || (map[i].y_coord > grid.height - 1) || (map[i].x_coord < 0) || (map[i].y_coord < 0))
                {
                    return 1;
                }
            }

            return 0;
        }

        private void reposition(List<shape_block> change_map, List<shape_block> orig_map, Field grid)
        {
            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                grid.area[orig_map[i].x_coord, orig_map[i].y_coord] = ' ';      //cleaning up figure trails  
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                orig_map[i] = new shape_block { x_coord = change_map[i].x_coord, y_coord = change_map[i].y_coord };
                grid.area[change_map[i].x_coord, change_map[i].y_coord] = '#';
            }

        }

        private void rotation_transformation(List<shape_block> change_map, List<shape_block> orig_map, Field grid)
        {
            int delta_x = orig_map[center].x_coord;         //placing shape block at (0,0) for rotation simplicity then placing it back where it was, delta = current coordinates of the figure        
            int delta_y = orig_map[center].y_coord;         //referring to the shape's centre

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                change_map.Add(new shape_block { x_coord = orig_map[i].x_coord - delta_x, y_coord = orig_map[i].y_coord - delta_y });
            }

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                change_map[i] = new shape_block { x_coord = change_map[i].y_coord, y_coord = SHAPE_SIZE - orig_map[i].x_coord };
            }
 
            int delta_new_x = Math.Abs(change_map[center].x_coord - delta_x); //offset - centre should be in the same location
            int delta_new_y = Math.Abs(change_map[center].y_coord - delta_y);

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                change_map[i] = new shape_block { x_coord = change_map[i].x_coord + delta_new_x, y_coord = change_map[i].y_coord + delta_new_y };
            }
        }


        public int rotate(Field grid)
        {

            if (shape_type == 3)            //square block can't rotate
                return 1;

            List<shape_block> rotation_map = new List<shape_block>();

            rotation_transformation(rotation_map, shape_map, grid);

            if (check_out_of_bounds(rotation_map, grid) == 1)
                return 1;

             if (check_collisions(rotation_map, shape_map, grid) == 1)
               return 1;

            reposition(rotation_map, shape_map, grid);
            return 0;

        }

        public int move_down(Field grid)
        {
            List<shape_block> move_down_map = new List<shape_block>();

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                move_down_map.Add(new shape_block { x_coord = shape_map[i].x_coord, y_coord = shape_map[i].y_coord + 1});
            }

            if((check_out_of_bounds(move_down_map, grid) == 1) || (check_collisions(move_down_map, shape_map, grid) == 1))      //bottom collision
            {
                check_blast(grid); //if the shape has fallen, check full rows
                if (shape_map_create(grid) == 1)        //if the field is full - game over
                {
                    grid.gameover = true;
                    return 2;
                }
                return 1;
            }

            reposition(move_down_map, shape_map, grid);
            return 0;
        }


        public int move_right(Field grid)
        {
            List<shape_block> move_right_map = new List<shape_block>();

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                move_right_map.Add(new shape_block { x_coord = shape_map[i].x_coord + 1, y_coord = shape_map[i].y_coord });
            }

            if (check_out_of_bounds(move_right_map, grid) == 1)
                return 1;

            if (check_collisions(move_right_map, shape_map, grid) == 1)
                return 1;

            reposition(move_right_map, shape_map, grid);
            return 0;
        }

        public int move_left(Field grid)
        {
            List<shape_block> move_left_map = new List<shape_block>();

            for (int i = 0; i < SHAPE_SIZE; i++)
            {
                move_left_map.Add(new shape_block { x_coord = shape_map[i].x_coord - 1, y_coord = shape_map[i].y_coord });
            }

            if (check_out_of_bounds(move_left_map, grid) == 1)
                return 1;

            if (check_collisions(move_left_map, shape_map, grid) == 1)
                return 1;

            reposition(move_left_map, shape_map, grid);
            return 0;

        }


    }
}