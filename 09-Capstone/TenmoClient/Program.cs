using System;
using TenmoClient.Data;
using TenmoClient.Models;

namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        

        static void Main(string[] args)
        {
            Run();
        }
        private static void Run()
        {
            while (true)
            {
                int loginRegister = -1;
                while (loginRegister != 1 && loginRegister != 2)
                {
                    Console.WriteLine("Welcome to TEnmo!");
                    Console.WriteLine("1: Login");
                    Console.WriteLine("2: Register");
                    Console.WriteLine("0: Exit");
                    Console.Write("Please choose an option: ");

                    if (!int.TryParse(Console.ReadLine(), out loginRegister))
                    {
                        Console.WriteLine("Invalid input. Please enter only a number.");
                    }
                    else if (loginRegister == 0)
                    {
                        Environment.Exit(0);
                    }
                    else if (loginRegister == 1)
                    {
                        while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                        {
                            LoginUser loginUser = consoleService.PromptForLogin();
                            API_User user = authService.Login(loginUser);
                            if (user != null)
                            {
                                UserService.SetLogin(user);
                            }
                        }
                    }
                    else if (loginRegister == 2)
                    {
                        bool isRegistered = false;
                        while (!isRegistered) //will keep looping until user is registered
                        {
                            LoginUser registerUser = consoleService.PromptForLogin();
                            isRegistered = authService.Register(registerUser);
                            if (isRegistered)
                            {
                                Console.WriteLine("");
                                Console.WriteLine("Registration successful. You can now log in.");
                                loginRegister = -1; //reset outer loop to allow choice for login
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection.");
                    }
                }

                MenuSelection();
            }
        }

        private static void MenuSelection()
        {
            AccountService accountService = new AccountService();
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine("1: View your current balance");
                Console.WriteLine("2: View your past transfers");
                Console.WriteLine("3: View your pending requests");
                Console.WriteLine("4: Send TE bucks");
                Console.WriteLine("5: Request TE bucks");
                Console.WriteLine("6: Log in as different user");
                Console.WriteLine("0: Exit");
                Console.WriteLine("---------");
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                    menuSelection = -1;
                }
                else if (menuSelection == 1)
                {
                    Console.WriteLine($"\nYour current account balance is {accountService.GetAccountBalance():C}\n");


                }
                else if (menuSelection == 2)
                {
                    accountService.ListAllTransfers();

                }
                else if (menuSelection == 3)
                {
                    // View your pending requests

                }
                else if (menuSelection == 4)
                {
                    //TODO: format (padding) 
                    Console.WriteLine("User ID      Username");
                    accountService.DisplayUsers();
                    Console.WriteLine("Please enter the ID of the user you'd like to transfer to:");
                    int toUserID = int.Parse(Console.ReadLine()); //hold toUserID variable
                    if (!accountService.ConfirmRecipient(toUserID)) //make sure the to_account_id exists
                    {
                        Console.WriteLine("\nSorry, that USER ID does not exist.\n");
                        //TODO: return user to this menu, not the main menu. 
                        MenuSelection();
                        
                    }
                    else if (toUserID == UserService.GetUserId())
                    {
                        Console.WriteLine("\nSorry, you cannot send money to yourself!\n");
                        
                        MenuSelection();
                    }
                    Console.WriteLine("\nPlease enter the amount you would like to send:");
                    decimal amount = decimal.Parse(Console.ReadLine());  //make sure the from_account has enough funds and hold amount variable
                    if (accountService.GetAccountBalance() < amount)
                    {
                        Console.WriteLine("\nSorry, you don't have enough funds.\n");
                       
                        MenuSelection();
                        
                    }
                    Transfer newTransfer = new Transfer(UserService.GetUserId(), toUserID, amount);
                    accountService.CompleteTransfer(newTransfer);


                }
                else if (menuSelection == 5)
                {
                    // Request TE bucks

                }
                else if (menuSelection == 6)
                {
                    // Log in as different user
                    Console.WriteLine("");
                    UserService.SetLogin(new API_User()); //wipe out previous login info
                    return; //return to entry point
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}
