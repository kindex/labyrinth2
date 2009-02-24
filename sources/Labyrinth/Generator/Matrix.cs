using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Labyrinth.Generator
{
    class Matrix
    {
	    struct Node
	    {
		    public bool right;
            public bool up;
	    };

        // first index - x, second - y
        Node[,] matrix;

        public Matrix(int x, int y)
        {
            matrix = new Node[x, y];
        }

        public bool isRightBorder(int x, int y)
        {
            if (!(x < dim_x && y >= 0))
            {
                return false;
            }
            if ((x < 0 || y < 0))
            {
                return true;
            }
            return y + 1 > dim_y
                || x + 1 >= dim_x
                || matrix[x, x].right;
        }

        public bool isUpBorder(int x, int y)
        {
            if (!(y < dim_y && x >= 0))
            {
                return false;
            }
            if (x < 0 || y < 0)
            {
                return true;
            }
            return y + 1 >= dim_y
                || x >= dim_x
                || matrix[x, y].up;
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

        public void setBorders(int x, int y, bool right, bool up)
        {
            matrix[x, y].right = right;
            matrix[x, y].up = up;
        }
    }
}



//class Labyrinth
//{
//public:
//    bool isRightBorder(int x, int y) const;
//    bool isUpBorder(int x, int y) const;
//    int getMaxX() const;
//    int getMaxY() const;
//    void setSize(size_t x, size_t y);

//    friend class LabyrinthGenerator;
//    std::string serialize() const;
//    void unserialize(int x, int y, const std::string& data);

//private:
//    struct node
//    {
//        bool rightborder;
//        bool upborder;
//    };

//// first index - y, second - x

//    std::vector<std::vector<node> > a;
//};
