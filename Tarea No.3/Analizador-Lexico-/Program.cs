using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class AnalizadorLexico
{
    static Dictionary<string, string> tablaTokens = new Dictionary<string, string>();

    static HashSet<string> palabrasReservadas = new HashSet<string>
    {
        "if", "else", "while", "for", "int", "float", "double", "string", "return"
    };

    static void Main()
    {
        string codigo = LeerArchivo();

        string codigoFuente = @"
            // Esto es un comentario de línea
            int suma = contador + 40;
            /* Comentario
               multilinea */
            if (suma >= 50) {
                suma = suma + 1;
            }
            string Nombre = \""Keily\""
        ";

        Analizar(codigo);
        MostrarTabla();
    }

    static string LeerArchivo()
    {
        string ubicacion = string.Empty;
        Console.WriteLine("Ingrese Archivo");
        ubicacion = Console.ReadLine();

        string contenido =  File.ReadAllText(ubicacion);
        return contenido;

    }


    static void Analizar(string codigo)
    {
        codigo = Regex.Replace(codigo, @"//.*", "");
        codigo = Regex.Replace(codigo, @"/\*[\s\S]*?\*/", "");

        string patron = @"(<=|>=|==|!=|[{}();=+\-*/<>])|\b\d+(\.\d+)?\b|\b[a-zA-Z_]\w*\b|";

        MatchCollection coincidencias = Regex.Matches(codigo, patron);

        foreach (Match match in coincidencias)
        {
            string lexema = match.Value;

            if (!tablaTokens.ContainsKey(lexema))
            {
                tablaTokens[lexema] = ObtenerToken(lexema);
            }
        }
    }

    static string ObtenerToken(string lexema)
    {
        if (palabrasReservadas.Contains(lexema))
            return "PALABRA_RESERVADA";

        if (Regex.IsMatch(lexema, @"^\d+$"))
            return "NUM_ENTERO";

        if (Regex.IsMatch(lexema, @"^\d+\.\d+$"))
            return "NUM_REAL";

        if (Regex.IsMatch(lexema, @"""(\\.|[^""\\])*"""))
            return "CADENA";
        
        if (Regex.IsMatch(lexema, @"^[a-zA-Z_]\w*$"))
            return "IDENTIFICADOR";


        switch (lexema)
        {
            case "=": return "OP_ASIGNACION";
            case "+": return "OP_SUMA";
            case "-": return "OP_RESTA";
            case "*": return "OP_MULTIPLICACION";
            case "/": return "OP_DIVISION";
            case "<": return "OP_MENOR";
            case ">": return "OP_MAYOR";
            case "<=": return "OP_MENOR_IGUAL";
            case ">=": return "OP_MAYOR_IGUAL";
            case "==": return "OP_IGUAL";
            case "!=": return "OP_DIFERENTE";
            case ";": return "PUNTO_COMA";
            case "(": return "PARENTESIS_ABRE";
            case ")": return "PARENTESIS_CIERRA";
            case "{": return "LLAVE_ABRE";
            case "}": return "LLAVE_CIERRA";
        }

        return "TOKEN_DESCONOCIDO";
    }

    static void MostrarTabla()
    {
        Console.WriteLine("TABLA LEXEMA | TOKEN\n");

        foreach (var item in tablaTokens)
        {
            Console.WriteLine($"{item.Key}  |  {item.Value}");
        }
    }
}
