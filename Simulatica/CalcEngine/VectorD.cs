using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace CalcEngine
{
    /// <summary>
    /// TODO: Reflect (collision management)
    /// Vec4 operator ^.
    /// </summary>
    public partial struct VectorD3{

        public double X, Y, Z;

        public VectorD3(double x, double y, double z)
        {
            X = x; Y = y; Z = z;
        }


        #region operators
        //[JitIntrinsic]
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorD3 operator +(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }
        public static VectorD3 operator -(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }
        public static VectorD3 operator *(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }
        public static VectorD3 operator *(VectorD3 vec, double scalar)
        {
            return new VectorD3(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
        }
        public static VectorD3 operator *(double scalar, VectorD3 vec)
        {
            return new VectorD3(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
        }
        public static VectorD3 operator ^(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.Y*right.Z - left.Z*right.Y, left.Z * right.X - left.X * right.Z, left.X * right.Y - left.Y * right.X);
        }
        public static VectorD3 operator /(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }
        #endregion

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public static double Distance(VectorD3 value1, VectorD3 value2)
        {
            double dx = value1.X - value2.X;
            double dy = value1.Y - value2.Y;
            double dz = value1.Z - value2.Z;

            double ls = dx * dx + dy * dy + dz * dz;

            return Math.Sqrt(ls);
        }

        public VectorD3 Normalized()
        {
            double tmp = this.Length();
            return new VectorD3(this.X/tmp, this.Y / tmp, this.Z / tmp);
        }

    }



    public partial struct VectorD4
    {

        public double X, Y, Z, W;

        public VectorD4(double x, double y, double z, double w)
        {
            X = x; Y = y; Z = z; W = w;
        }


        #region operators
        //[JitIntrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static VectorD4 operator +(VectorD4 left, VectorD4 right)
        {
            return new VectorD4(left.X + right.X, left.Y + right.Y, left.Z + right.Z, left.W + right.W);
        }
        public static VectorD4 operator -(VectorD4 left, VectorD4 right)
        {
            return new VectorD4(left.X - right.X, left.Y - right.Y, left.Z - right.Z, left.W - right.W);
        }
        public static VectorD4 operator *(VectorD4 left, VectorD4 right)
        {
            return new VectorD4(left.X * right.X, left.Y * right.Y, left.Z * right.Z, left.W * right.W);
        }
        public static VectorD4 operator *(VectorD4 vec, double scalar)
        {
            return new VectorD4(vec.X * scalar, vec.Y * scalar, vec.Z * scalar, vec.W * scalar);
        }
        public static VectorD4 operator *(double scalar, VectorD4 vec)
        {
            return new VectorD4(vec.X * scalar, vec.Y * scalar, vec.Z * scalar, vec.W * scalar);
        }
        public static VectorD4 operator /(VectorD4 left, VectorD4 right)
        {
            return new VectorD4(left.X / right.X, left.Y / right.Y, left.Z / right.Z, left.W / right.W);
        }
        #endregion

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
        }

        public static double Distance(VectorD4 value1, VectorD4 value2)
        {
            double dx = value1.X - value2.X;
            double dy = value1.Y - value2.Y;
            double dz = value1.Z - value2.Z;
            double dw = value1.W - value2.W;

            double ls = dx * dx + dy * dy + dz * dz + dw * dw;

            return Math.Sqrt(ls);
        }

        public VectorD4 Normalized()
        {
            double tmp = this.Length();
            return new VectorD4(this.X / tmp, this.Y / tmp, this.Z / tmp, this.W / tmp);
        }

    }



    /*
    public partial struct Vector3<T> : ISummable<double>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }


        #region operators

        T Sum<T>(T a, T b) where
    T : ISummable<T>
        {
            return a.Add(a, b);
        }


        
        public static Vector3<T> operator +(Vector3<T> left, Vector3<T> right)
        {
            return new Vector3<T>(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }
        

        public static VectorD3 operator -(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }
        public static VectorD3 operator *(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X * right.X, left.Y * right.Y, left.Z * right.Z);
        }
        public static VectorD3 operator *(VectorD3 vec, double scalar)
        {
            return new VectorD3(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
        }
        public static VectorD3 operator *(double scalar, VectorD3 vec)
        {
            return new VectorD3(vec.X * scalar, vec.Y * scalar, vec.Z * scalar);
        }
        public static VectorD3 operator /(VectorD3 left, VectorD3 right)
        {
            return new VectorD3(left.X / right.X, left.Y / right.Y, left.Z / right.Z);
        }
        
        #endregion
    }*/
}
