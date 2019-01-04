using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.AccountManagement;

namespace ActiveDirectoryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Característica que será agregada a la rama Experimental.
            // Segunda Característica que será agregada a la rama Experimental.

            Console.Write("Escriba su nombre de usuario: ");
            string username = Console.ReadLine();

            Console.Write("Escriba su contraseña: ");
            string password = ReadPassword();

            // 1) Conexión al Active Directory
            using (var activeDirectory = new PrincipalContext(ContextType.Domain, "GMX.COM.MX")) {
                try {
                    // 2) Validar credenciales (Autenticación)
                    bool authenticated = activeDirectory.ValidateCredentials(username, password);


                    if (authenticated) {
                        Console.WriteLine("\n¡Autenticado!");

                        // 3) Ver los roles a los que pertenece el usuario (Autorización)
                        var user = UserPrincipal.FindByIdentity(activeDirectory, username);
                        var groups = user.GetGroups();

                        bool isInGMXPapercut = user.IsMemberOf(GroupPrincipal.FindByIdentity(activeDirectory, "GMXPAPERCUT"));
                        bool isInInternetDesarrollo = user.IsMemberOf(GroupPrincipal.FindByIdentity(activeDirectory, "Internet_Desarrollo"));

                        Console.WriteLine($"Is in GMXPAPERCUT? {isInGMXPapercut}");
                        Console.WriteLine($"Is in Internet_Desarrollo? {isInInternetDesarrollo}\n");
                        Console.WriteLine($"El usuario '{username}' pertenece a los siguientes grupos:\n");

                        foreach (var group in groups) {
                            Console.WriteLine(group);
                        }
                    } else {
                        Console.WriteLine("\n\nBad Login");
                    }
                } catch (Exception e) {
                    Console.WriteLine("ERROR:\n\n" + e);
                }
            }

            Console.WriteLine("Presione cualquier tecla para continuar.");
            Console.ReadKey();
        }

        static string ReadPassword()
        {
            var builder = new StringBuilder();

            do {
                var key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter) {
                    builder.Append(key.KeyChar);
                    Console.Write('*');
                } else {
                    if (key.Key == ConsoleKey.Backspace && builder.Length > 0) {
                        builder.Remove(builder.Length - 1, 1);
                        Console.Write("\b \b");
                    } else if (key.Key == ConsoleKey.Enter) {
                        break;
                    }
                }
            } while (true);

            return builder.ToString();
        }
    }
}
