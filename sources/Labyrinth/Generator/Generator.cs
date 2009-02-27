using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Labyrinth.Generator
{
    static class Generator
    {
        sealed class LabyrinthGenerator
        {
            class Node
            {
                public int dist;
                public bool path;
                public bool rightborder;
                public bool upborder;
                public bool was;
                public int next_x;
                public int next_y;
            }
            Node[,] matrix;
            static Random random = new Random();

            public LabyrinthGenerator(int x, int y)
            {
                matrix = new Node[x, y];
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        matrix[i, j] = new Node();
                        matrix[i, j].dist = int.MaxValue;
                    }
                }
            }

            public void Build(float density)
            {
                if (dim_x > 0 && dim_y > 0)
                {
	                Deep(0, 0, 0);
                }
                findpath(dim_x - 1, dim_y - 1);
	            //findpath(X-1, 0);
	            //findpath(0, Y-1);

                //string d = "\n";

                //for(int i = 0; i < dim_y; i++)
                //{
                //    d += "|";
                //    for(int j = 0; j < dim_x; j++)
                //    {
                //        d += "\t" + IntToStr(a[i][j].dist);
                //    }
                //    d += "|\n";
                //}
                //log_msg("labyrinth", d);

	            for(int y = 0; y < dim_y; y++)
	            {
		            for(int x = 0; x < dim_x-1; x++)
		            {
                        matrix[x, y].rightborder = random.NextDouble() > density && Math.Abs(matrix[x, y].dist - matrix[x + 1, y].dist) != 1;
		            }
	            }
	            for(int y = 0; y < dim_y-1; y++)
	            {
		            for(int x = 0; x < dim_x; x++)
		            {
                        matrix[x, y].upborder = random.NextDouble() > density && Math.Abs(matrix[x, y].dist - matrix[x, y + 1].dist) != 1;
		            }
	            }
            }

            public Matrix getMatrix()
            {
                Matrix result = new Matrix(matrix.GetLength(0), matrix.GetLength(1));
                for (int x = 0; x < dim_x; x++)
                {
                    for (int y = 0; y < dim_y; y++)
                    {
                        int next_x;
                        int next_y;
                        if (matrix[x, y].path && !(x == dim_x -1 && y == dim_y -1))
                        {
                            next_x = matrix[x, y].next_x;
                            next_y = matrix[x, y].next_y;
                        }
                        else
                        {
                            next_x = x;
                            next_y = y;
                        }
                        result.setCell(x, y, matrix[x, y].rightborder, matrix[x, y].upborder, next_x, next_y);
                    }
                }

                return result;
            }

            public int dim_y
            {
                get
                {
                    return matrix.GetLength(0);
                }
            }

            public int dim_x
            {
                get
                {
                    return matrix.GetLength(1);
                }
            }

            bool Deep(int x, int y, int level)
            {
                Node current = matrix[x, y];
	            if (current.was)
	            {
		            return false;
	            }
	            current.dist = level;
	            current.was = true;
	            if (x == dim_x-1 && y == dim_y-1)
	            {
		            return true;
	            }

	            bool[] used_directions = {false, false, false, false};
	            int total = 0;

	            while (total < 4)
	            {
		            int d;
		            do
		            {
                        d = random.Next(0, 5) % 4;
		            }
		            while (used_directions[d]);
		            total++;

		            used_directions[d] = true;

		            switch(d)
		            {
			            case 0: 
				            if (x > 0)
				            {
					            Deep(x - 1, y, level + 1);
				            }
				            break;
			            case 1:
				            if (y > 0)
				            {
					            Deep(x, y - 1, level + 1); 
				            }
				            break;
			            case 2:
				            if (x < dim_x-1)
				            {
					            Deep(x + 1, y, level + 1);
				            }
				            break;
			            case 3:
				            if (y < dim_y-1)
				            {
					            Deep(x, y + 1, level + 1); 
				            }
				            break;
		            }
	            }
	            return false;
            }

            void findpath(int x, int y)
            {
                Node cur_node = matrix[x, y];
                cur_node.path = true;
                int d = cur_node.dist - 1;
                if (y > 0 && matrix[x, y - 1].dist == d)
                {
                    matrix[x, y - 1].next_x = x;
                    matrix[x, y - 1].next_y = y;
                    findpath(x, y - 1);
                }
                else if (x > 0 && matrix[x - 1, y].dist == d)
                {
                    matrix[x - 1, y].next_x = x;
                    matrix[x - 1, y].next_y = y;
                    findpath(x - 1, y);
                }
                else if (y < dim_y - 1 && matrix[x, y + 1].dist == d)
                {
                    matrix[x, y + 1].next_x = x;
                    matrix[x, y + 1].next_y = y;
                    findpath(x, y + 1);
                }
                else if (x < dim_x - 1 && matrix[x + 1, y].dist == d)
                {
                    matrix[x + 1, y].next_x = x;
                    matrix[x + 1, y].next_y = y;
                    findpath(x + 1, y);
                }
            }

        }

        public static Matrix Generate(int x, int y, float density)
        {
            LabyrinthGenerator generator = new LabyrinthGenerator(x, y);
            generator.Build(density);

            return generator.getMatrix();
        }
    }
}
