public class OperacionesMatriciales : IOperacionesMatricias
{
    // Método para calcular la inversa de una matriz
    public void Inversa(string nombre, string nombreInversa)
    {
        IMatriz A = new Matriz();
        IMatriz S = new Matriz();
        IMatriz AMP = new Matriz();
        int[] Aux;

        // Verificar si la matriz es singular
        if (A.MakeDeterminante(nombre) == 0)
            throw new Exception("La matriz es singular");

        Aux = new int[A.Count];

        // Copiar los elementos de A a AMP
        for(int i = 0; i < A.Count; i++)
        {
            for (int j = 0; j < A.Columns; j++)
            {
                AMP.Elements[i, j] = A.Elements[i, j];
            }
        }

        // Inicializar matriz aumentada
        AMP.Count = A.Count;
        AMP.Columns = 2 * A.Columns;
        AMP.Elements = new double[AMP.Count, AMP.Columns];

        // Crear la matriz aumentada
        MakeAMP(A, AMP);

        // Aplicar eliminación gaussiana para triangularizar la matriz aumentada
        TriangularINF(AMP, Aux);
        TriangularSup(AMP);

        // Normalizar la matriz triangular superior
        Ones(AMP);

        // Extraer la inversa de la matriz aumentada
        S.Count = A.Count;
        S.Columns = A.Count;
        S.Elements = new double[S.Count, S.Columns];
        MakeS(AMP, S, Aux);

        // Guardar la inversa en un archivo
        IDirectorio D = new Directorio();
        D.Save(nombreInversa, Mat(S));
    }

    // Método para multiplicar dos matrices y guardar el resultado en un archivo
    public void Multiplicar(string nombre1, string nombre2, string nombreProducto)
    {
        IMatriz A = new Matriz();
        IMatriz B = new Matriz();
        IMatriz C = new Matriz();

        // Validar las matrices de entrada
        A.Validate(nombre1);
        B.Validate(nombre2);

        // Verificar compatibilidad de las matrices para la multiplicación
        if (A.Columns != B.Count)
            throw new System.Exception("Error: el número de columnas de A difiere al número de filas de B");

        // Inicializar matriz de resultado
        C.Count = A.Count;
        C.Columns = B.Columns;
        C.Elements = new double[C.Count, C.Columns];

        // Realizar la multiplicación de matrices
        string Mat = MMult(A, B, C);

        // Guardar el resultado en un archivo
        IDirectorio ID = new Directorio();
        ID.Save(nombreProducto, Mat);
    }

    // Método para calcular la potencia de una matriz y guardar el resultado en un archivo
    public void Potencia(string nombre, int exponente, string nombrePotencia)
    {
        IMatriz A = new Matriz();
        IMatriz S = new Matriz();
        
        // Validar la matriz de entrada
        A.Validate(nombre);

        // Verificar si la matriz es cuadrada
        if (A.Count != A.Columns)
            throw new System.Exception("Error: la matriz A no es cuadrada");

        // Inicializar matriz identidad
        S.Count = A.Count; S.Columns = A.Columns;
        S.Elements = new double[S.Count, S.Columns];
        MatrizUnidad(S);

        // Calcular la potencia de la matriz
        S = CalcularPotencia(exponente, A, S);

        // Guardar el resultado en un archivo
        Save(S, nombrePotencia);
    }

    // Método para sumar o restar dos matrices y guardar el resultado en un archivo
    public void Suma(string nombre1, string nombre2, string nombresuma, int signo)
    {
        IMatriz A = new Matriz();
        IMatriz B = new Matriz();
        IMatriz C = new Matriz();

        // Validar las matrices de entrada
        A.Validate(nombre1);
        B.Validate(nombre2);

        // Verificar compatibilidad de las matrices para la suma/resta
        if (A.Count != B.Count || A.Columns != B.Columns)
            throw new System.Exception("Error: no es posible la suma o resta en estas matrices");

        // Inicializar matriz de resultado
        C.Count = A.Count;
        C.Columns = A.Columns;
        C.Elements = new double[C.Count, C.Columns];

        // Realizar la suma o resta de matrices
        string Mat = "[\n";
        for (int i = 0; i < C.Count; i++)
        {
            for (int j = 0; j < C.Columns; j++)
            {
                C.Elements[i, j] = A.Elements[i, j] + signo * B.Elements[i, j];
                Mat += C.Elements[i, j] + " ";
            }
            Mat = Mat.Remove(Mat.Length - 1) + ";\n";
        }
        Mat = Mat.Remove(Mat.Length - 2) + "\n]";

        // Guardar el resultado en un archivo
        IDirectorio ID = new Directorio();
        ID.Save(nombresuma, Mat);
    }

    // Método para inicializar una matriz como la matriz identidad
    private void MatrizUnidad(IMatriz S)
    {
        for (int i = 0; i < S.Count; i++)
        {
            for (int j = 0; j < S.Columns; j++)
            {
                if (i == j) S.Elements[i, j] = 1;
                else S.Elements[i, j] = 0;
            }
        }
    }

    // Método para calcular la potencia de una matriz mediante multiplicaciones sucesivas
    private Matriz CalcularPotencia(int exp, Matriz A, Matriz S)
    {
        for (int i = 0; i < exp; i++)
        {
            Matriz C = new Matriz();
            C.Count = A.Count; C.Columns = A.Columns;
            C.Elements = new double[C.Count, C.Columns];
            MMult(A, S, C);
            S = C;
        }
        return S;
    }

    // Método para realizar la multiplicación de matrices y formatear el resultado en una cadena
    private string MMult(IMatriz A, IMatriz B, IMatriz C)
    {
        string Mat = "[\n";
        for (int i = 0; i < C.Count; i++)
        {
            for (int j = 0; j < C.Columns; j++)
            {
                C.Elements[i, j] = 0;
                for (int k = 0; k < A.Columns; k++)
                {
                    C.Elements[i, j] += A.Elements[i, k] * B.Elements[k, j];
                }
                C.Elements[i, j] = Math.Round(C.Elements[i, j], 12);
                if (Math.Abs(C.Elements[i, j]) < 1E-14) C.Elements[i, j] = 0;
                Mat += C.Elements[i, j] + " ";
            }
            Mat = Mat.Remove(Mat.Length - 1) + ";\n";
        }
        Mat = Mat.Remove(Mat.Length - 2) + "\n]";
        return Mat;
    }

    // Método para inicializar la matriz aumentada
    private void MakeAMP(IMatriz A, IMatriz AMP)
    {
        for (int i = 0; i < A.Count; i++)
        {
            for (int j = 0; j < A.Columns; j++)
            {
                AMP.Elements[i, j] = A.Elements[i, j];
                if (i == j) AMP.Elements[i, j + A.Columns] = 1;
                else AMP.Elements[i, j + A.Columns] = 0;
            }
        }
    }

    // Método para intercambiar filas en la matriz aumentada
    private void SwapAmp(int[] Aux, int posM, int pos0)
    {
        int im = -1, i0 = -1;
        for (int i = 0; i < Aux.Length; i++)
        {
            if (Aux[i] == posM) im = i;
            if (Aux[i] == pos0) i0 = i;
        }
        Aux[im] = pos0;
        Aux[i0] = posM;
    }

    // Método para intercambiar filas en una matriz
    private void Swap(IMatriz AMP, int[] Aux, int posM, int pos0)
    {
        SwapAmp(Aux, posM, pos0);
        double aux;
        for (int i = 0; i < AMP.Columns; i++)
        {
            aux = AMP.Elements[pos0, i];
            AMP.Elements[pos0, i] = AMP.Elements[posM, i];
            AMP.Elements[posM, i] = aux;
        }
    }

    // Método para encontrar el mayor elemento en una columna y realizar intercambio de filas
    private void HallarMayor(IMatriz AMP, int[] Aux, int pos0)
    {
        int posM = pos0;
        for (int i = pos0; i < AMP.Count; i++)
        {
            if (AMP.Elements[i, pos0] > AMP.Elements[posM, pos0])
            {
                posM = i;
            }
        }

        // Si el mayor elemento no es el de la fila actual, se realiza el intercambio
        if (posM != pos0)
        {
            Swap(AMP, Aux, posM, pos0);
        }
    }

    // Método para triangularizar la parte inferior de la matriz aumentada
    private void TriangularINF(IMatriz AMP, int[] Aux)
    {
        for (int i = 0; i < AMP.Count; i++)
        {
            HallarMayor(AMP, Aux, i);
            for (int j = i + 1; j < AMP.Count; j++)
            {
                double m = AMP.Elements[j, i] / AMP.Elements[i, i];
                for (int k = 0; k < AMP.Columns; k++)
                {
                    AMP.Elements[j, k] = AMP.Elements[j, k] - m * AMP.Elements[i, k];
                }
            }
        }
    }

    // Método para formatear la matriz en una cadena
    private string Mat(IMatriz AMP)
    {
        string Mat = "[\n";
        for (int i = 0; i < AMP.Count; i++)
        {
            for (int j = 0; j < AMP.Columns; j++)
            {
                Mat += AMP.Elements[i, j] + " ";
            }
            Mat = Mat.Remove(Mat.Length - 1) + ";\n";
        }
        Mat = Mat.Remove(Mat.Length - 2) + "\n]";
        return Mat;
    }

    // Método para triangularizar la parte superior de la matriz aumentada
    private void TriangularSup(IMatriz AMP)
    {
        for (int i = AMP.Count - 1; i >= 0; i--)
        {
            for (int j = i - 1; j >= 0; j--)
            {
                double m = AMP.Elements[j, i] / AMP.Elements[i, i];
                for (int k = 0; k < AMP.Columns; k++)
                {
                    AMP.Elements[j, k] -= m * AMP.Elements[i, k];
                }
            }

            // Paso de escalado diagonal
            for (int k = 0; k < AMP.Columns; k++)
            {
                AMP.Elements[i, k] /= AMP.Elements[i, i];
            }
        }
    }

    // Método para normalizar la diagonal de la matriz aumentada
    private void Ones(IMatriz AMP)
    {
        for (int i = 0; i < AMP.Count; i++)
        {
            double m = AMP.Elements[i, i];
            for (int j = 0; j < AMP.Columns; j++)
            {
                AMP.Elements[i, j] = AMP.Elements[i, j] /= m;
            }
        }
    }

    // Método para extraer la inversa de la matriz aumentada
    private void MakeS(IMatriz AMP, IMatriz S, int[] Aux)
    {
        for (int i = 0; i < AMP.Count; i++)
        {
            for (int j = 0; j < AMP.Count; j++)
            {
                S.Elements[i, j] = AMP.Elements[i, j + AMP.Count];
            }
        }
    }
}
