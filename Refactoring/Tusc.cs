using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        private DataStore dataStore;
        private TuscUser loggedInUser;

        public Tusc(DataStore dataStoreIn)
        {
            this.dataStore = dataStoreIn;
            this.loggedInUser = null;
        }

        public void start()
        {
            showStartMessage();
            logIn();

            if (loggedInUser != null)
            {
                Console.Clear();
                showUserWelcomeMessage();

                bool userSelectionIsValid = true;
                while (userSelectionIsValid)
            {
                    showProductList();
                    int userSelection = getIntFromUser("Enter a number:");

                    userSelectionIsValid = userSelection > 0 && userSelection <= dataStore.Products.Count;
                    if (userSelectionIsValid)
                {
                        processUserSelection(userSelection);
                    }
                }
            }

            dataStore.updateUser(loggedInUser);
            dataStore.writeToDisk();

            ConfirmExit();
        }
              

        
        private static void showStartMessage()
                {
            Console.WriteLine("Welcome to TUSC\n");
            Console.WriteLine("---------------");
        }

        private void logIn()
                    {
            while (loggedInUser == null)
            {
                string name = promptForUserName();

                if (!string.IsNullOrEmpty(name))
                        {
                    string pwd = GetUserInputFromConsole("Enter Password:");
                    loggedInUser = dataStore.Users.getValidUser(name, pwd);
                        }
                else
                {
                    break;
                    }

                        Console.Clear();
                if (loggedInUser == null)
                        {
                    writeMessage("\nYou entered an invalid username or password.", ConsoleColor.Red);
                }
            }
        }

        private static string promptForUserName()
                            {
            return GetUserInputFromConsole("\nEnter Username:");
        }

        private void showUserWelcomeMessage()
        {
            writeMessage(string.Format("\nLogin successful! Welcome {0}!", loggedInUser.Name), ConsoleColor.Green);
            Console.WriteLine("\nYour balance is " + loggedInUser.Balance.ToString("C"));
                            }

        private void showProductList()
                        {
            Console.WriteLine("\nWhat would you like to buy?");
            for (int i = 0; i < dataStore.Products.Count; i++)
                            {
                Product prod = dataStore.Products[i];
                                Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
                            }
            Console.WriteLine(dataStore.Products.Count + 1 + ": Exit");
        }

        private void processUserSelection(int num)
        {
            int productIndex = num - 1;
            Console.WriteLine("\nYou want to buy: " + dataStore.Products[productIndex].Name);
            Console.WriteLine("Your balance is " + loggedInUser.Balance.ToString("C"));

            int qty = getIntFromUser("Enter amount to purchase:");

            bool userHasEnoughMoney = loggedInUser.Balance >= dataStore.Products[productIndex].Price * qty;
            bool storeHasEnoughProduct = dataStore.Products[productIndex].Qty >= qty;
            if (!userHasEnoughMoney)
                                    {
                showErrorMessage("\nYou do not have enough money to buy that.");
                                    }
            else if (!storeHasEnoughProduct)
            {
                showErrorMessage("\nSorry, " + dataStore.Products[productIndex].Name + " is out of stock");
                                }
            else if (qty == 0)
            {
                showWarningMessage("\nPurchase cancelled");
                            }
                            else
                            {
                loggedInUser.Balance -= dataStore.Products[productIndex].Price * qty;
                dataStore.Products[productIndex].Qty = dataStore.Products[productIndex].Qty - qty;

                showPurchaseMessage(dataStore.Products, loggedInUser, productIndex, qty);
            }
        }

        private static void showPurchaseMessage(List<Product> prods, TuscUser loggedInUser, int productIndex, int qty)
                                {
                                    Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You bought " + qty + " " + prods[productIndex].Name);
            Console.WriteLine("Your new balance is " + loggedInUser.Balance.ToString("C"));
                                    Console.ResetColor();
                                }

        private void ConfirmExit()
                                {
            Console.WriteLine("\nPress Enter key to exit");
            Console.ReadLine();
                                }

        private static string GetUserInputFromConsole(string prompt)
                                {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            return input;
        }

        private static int getIntFromUser(string prompt)
        {
            string answer = GetUserInputFromConsole(prompt);
            int qty = Convert.ToInt32(answer);
            return qty;
                                }

        private static void showErrorMessage(string error)
                                {
                                    Console.Clear();
            writeMessage(error, ConsoleColor.Red);
                    }

        private static void showWarningMessage(string warning)
                    {
                        Console.Clear();
            writeMessage(warning, ConsoleColor.Yellow);
        }

        private static void writeMessage(string message, ConsoleColor color)
                {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
                    Console.ResetColor();
        }
    }
}
