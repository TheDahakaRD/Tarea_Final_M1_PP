using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

class Program
{
    static async Task Main(string[] args)
    {
        var corredores = new List<Corredor>
        {
            new Corredor("Correos Rápidos"),
            new Corredor("Velocidad Divina"),
            new Corredor("Corre Camino"),
            new Corredor("Relámpago Fulminante"),
            new Corredor("El Impulso"),
            new Corredor("Rayo McQueen"),
            new Corredor("Viento en Popa"),
            new Corredor("Fuga Imparable")
        };

        var tasks = corredores.Select(corredor => SimularCarrera(corredor)).ToArray();

        // Esperar a que todas las tareas terminen
        await Task.WhenAll(tasks);

        // Mostrar los resultados
        var clasificados = corredores.OrderBy(c => c.Tiempo.TotalMilliseconds).ToList();
        Console.WriteLine("\nResultados de la Carrera:");
        for (int i = 0; i < clasificados.Count; i++)
        {
            var puesto = i + 1;
            var tiempo = clasificados[i].Tiempo;
            Console.WriteLine($"{puesto}° {clasificados[i].Nombre}: {tiempo.Minutes:D2}:{tiempo.Seconds:D2}:{tiempo.Milliseconds:D3}");
        }
    }

    static async Task SimularCarrera(Corredor corredor)
    {
        // Iniciar la carrera con un tiempo aleatorio
        var tiempoTotal = new Random().Next(120, 300) * 1000; // Tiempo entre 2 y 5 minutos en milisegundos

        var tareaCarrera = Task.Run(async () =>
        {
            // Simular el progreso de la carrera
            for (int i = 0; i < tiempoTotal; i += 1000)
            {
                // Simulando cada segundo de carrera
                await Task.Delay(1000);
            }

            corredor.Tiempo = TimeSpan.FromMilliseconds(tiempoTotal);
        });

        // Esperar explícitamente la finalización de la tarea
        await tareaCarrera.ContinueWith(async t =>
        {
            if (t.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine($"{corredor.Nombre} ha terminado la carrera!");
            }
            else if (t.Status == TaskStatus.Canceled)
            {
                Console.WriteLine($"{corredor.Nombre} no terminó la carrera.");
            }
        });
    }
}

class Corredor
{
    public string Nombre { get; set; }
    public TimeSpan Tiempo { get; set; }

    public Corredor(string nombre)
    {
        Nombre = nombre;
    }
}
