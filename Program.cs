using System;
using System.Collections.Generic;
using System.IO;


class Program
{
    static List<Pregunta> preguntas = new List<Pregunta>();

    static void Main(string[] args)
    {
        // Cargar preguntas desde el archivo
        if (File.Exists("preguntas.txt"))
        {
            using (StreamReader sr = new StreamReader("preguntas.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string enunciado = sr.ReadLine();
                    List<string> opciones = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        opciones.Add(sr.ReadLine());
                    }
                    string respuestaCorrecta = sr.ReadLine();

                    Pregunta pregunta = new Pregunta
                    {
                        Enunciado = enunciado,
                        Opciones = opciones,
                        RespuestaCorrecta = respuestaCorrecta
                    };

                    preguntas.Add(pregunta);
                }
            }
        }

        bool salir = false;

        while (!salir)
        {
            Console.WriteLine("==== Menú ====");
            Console.WriteLine("1. Hacer tipo test");
            Console.WriteLine("2. Introducir nueva pregunta tipo test");
            Console.WriteLine("3. Eliminar pregunta");
            Console.WriteLine("4. Listado de preguntas");
            Console.WriteLine("5. Salir");
            Console.Write("Seleccione una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    HacerTipoTest();
                    break;
                case "2":
                    IntroducirNuevaPregunta();
                    break;
                case "3":
                    EliminarPregunta();
                    break;
                case "4":
                    MostrarListadoPreguntas();
                    break;
                case "5":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción no válida. Intente de nuevo.");
                    break;
            }

            Console.WriteLine();
        }
    }

    static void HacerTipoTest()
    {
        if (preguntas.Count == 0)
        {
            Console.WriteLine("No hay preguntas disponibles. Introduzca preguntas antes de realizar el tipo test.");
            return;
        }

        Console.Clear(); // Limpiar la consola

        Console.WriteLine("==== Tipo Test ====");

        // Obtener preguntas aleatorias hasta un máximo de 20
        Random rnd = new Random();
        int preguntasMostradas = 0;
        int indicePregunta = -1;

        while (preguntasMostradas < 20 && preguntasMostradas < preguntas.Count)
        {
            if (indicePregunta == -1 || indicePregunta >= preguntas.Count)
            {
                // Obtener una nueva pregunta aleatoria
                indicePregunta = rnd.Next(preguntas.Count);
            }

            Pregunta pregunta = preguntas[indicePregunta];

            Console.WriteLine(pregunta.Enunciado);

            // Mostrar opciones
            for (int i = 0; i < pregunta.Opciones.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {pregunta.Opciones[i]}");
            }

            // Leer respuesta del usuario
            Console.Write("Introduzca su respuesta: ");
            string respuestaUsuario = Console.ReadLine();

            // Verificar respuesta
            if (pregunta.Opciones[int.Parse(respuestaUsuario) - 1] == pregunta.RespuestaCorrecta)
            {
                Console.WriteLine("¡Respuesta correcta!");
            }
            else
            {
                Console.WriteLine($"Respuesta incorrecta. La respuesta correcta es: {pregunta.RespuestaCorrecta}");
            }

            preguntasMostradas++;
            indicePregunta++;
        }

        if (preguntasMostradas >= 20)
        {
            Console.WriteLine("Se han mostrado 20 preguntas. Fin del tipo test.");
        }
        else
        {
            Console.WriteLine("No hay más preguntas disponibles. Fin del tipo test.");
        }
    }


    static void IntroducirNuevaPregunta()
    {
        Console.Clear(); // Limpiar la consola
        Console.WriteLine("==== Introducir nueva pregunta tipo test ====");
        Console.Write("Introduzca el enunciado de la pregunta: ");
        string enunciado = Console.ReadLine();

        List<string> opciones = new List<string>();
        for (int i = 1; i <= 4; i++)
        {
            Console.Write($"Introduzca la opción {i}: ");
            string opcion = Console.ReadLine();
            opciones.Add(opcion);
        }

        Console.Write("Introduzca la respuesta correcta (1-4): ");
        int respuestaCorrectaIndex = int.Parse(Console.ReadLine()) - 1;
        string respuestaCorrecta = opciones[respuestaCorrectaIndex];

        Pregunta nuevaPregunta = new Pregunta
        {
            Enunciado = enunciado,
            Opciones = opciones,
            RespuestaCorrecta = respuestaCorrecta
        };

        preguntas.Add(nuevaPregunta);

        // Escribir pregunta en el archivo
        using (StreamWriter sw = File.AppendText("preguntas.txt"))
        {
            sw.WriteLine(enunciado);
            foreach (string opcion in opciones)
            {
                sw.WriteLine(opcion);
            }
            sw.WriteLine(respuestaCorrecta);
        }

        Console.WriteLine("Pregunta agregada correctamente.");
    }

    static void EliminarPregunta()
    {
        Console.Clear(); // Limpiar la consola
        if (preguntas.Count == 0)
        {
            Console.WriteLine("No hay preguntas disponibles para eliminar.");
            return;
        }

        Console.WriteLine("==== Eliminar pregunta ====");
        Console.WriteLine("Lista de preguntas:");

        for (int i = 0; i < preguntas.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {preguntas[i].Enunciado}");
        }

        Console.Write("Seleccione el número de pregunta a eliminar: ");
        int numeroPregunta = int.Parse(Console.ReadLine());

        if (numeroPregunta < 1 || numeroPregunta > preguntas.Count)
        {
            Console.WriteLine("Número de pregunta no válido.");
            return;
        }

        preguntas.RemoveAt(numeroPregunta - 1);
        Console.WriteLine("Pregunta eliminada correctamente.");

        // Actualizar el archivo de preguntas
        ActualizarArchivoPreguntas();
    }

    static void MostrarListadoPreguntas()
    {
        Console.Clear(); // Limpiar la consola
        if (preguntas.Count == 0)
        {
            Console.WriteLine("No hay preguntas disponibles.");
            return;
        }

        Console.WriteLine("==== Listado de preguntas ====");
        Console.WriteLine("Lista de preguntas:");

        for (int i = 0; i < preguntas.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {preguntas[i].Enunciado}");
        }

        Console.Write("Seleccione el número de pregunta para mostrar detalles: ");
        int numeroPregunta = int.Parse(Console.ReadLine());

        if (numeroPregunta < 1 || numeroPregunta > preguntas.Count)
        {
            Console.WriteLine("Número de pregunta no válido.");
            return;
        }

        Pregunta preguntaSeleccionada = preguntas[numeroPregunta - 1];

        Console.WriteLine("Detalles de la pregunta:");
        Console.WriteLine(preguntaSeleccionada.Enunciado);
        for (int i = 0; i < preguntaSeleccionada.Opciones.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {preguntaSeleccionada.Opciones[i]}");
        }
        Console.WriteLine($"Respuesta correcta: {preguntaSeleccionada.RespuestaCorrecta}");
    }

    static void ActualizarArchivoPreguntas()
    {
        // Sobreescribir el archivo de preguntas
        using (StreamWriter sw = new StreamWriter("preguntas.txt"))
        {
            foreach (Pregunta pregunta in preguntas)
            {
                sw.WriteLine(pregunta.Enunciado);
                foreach (string opcion in pregunta.Opciones)
                {
                    sw.WriteLine(opcion);
                }
                sw.WriteLine(pregunta.RespuestaCorrecta);
            }
        }
    }
}

public class Pregunta
{
    public string Enunciado { get; set; }
    public List<string> Opciones { get; set; }
    public string RespuestaCorrecta { get; set; }
}