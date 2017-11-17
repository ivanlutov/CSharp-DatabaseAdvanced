namespace P01_BillsPaymentSystem.App
{
    using System.Linq;
    using Data;
    using System;
    using Microsoft.EntityFrameworkCore;
    using Data.Models;

    public class Startup
    {
        public static void Main()
        {
            //Create Database and seed
            using (var context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
                Seed(context);
            }

            //User Details
            using (var context = new BillsPaymentSystemContext())
            {
                UserDetailsById(context);
            }

            //Pay Bills
            using (var context = new BillsPaymentSystemContext())
            {
                Console.Write("Enter userId: ");
                var userId = int.Parse(Console.ReadLine());
                Console.Write("Enter amount for bill pays: ");
                var amount = decimal.Parse(Console.ReadLine());

                try
                {
                    PayBills(userId, amount, context);
                    Console.WriteLine("Bills have been successfully paid.");
                }
                catch (InvalidOperationException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void PayBills(int userId, decimal amount, BillsPaymentSystemContext context)
        {
            var user = context.Users.Find(userId);

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                return;
            }

            decimal allАvailableMoneyOfUser = 0m;

            var bankAccounts = context
                .BankAccounts.Join(context.PaymentMethods,
                    (ba => ba.BankAccountId),
                    (p => p.BankAccountId),
                    (ba, p) => new
                    {
                        UserId = p.UserId,
                        BankAccountId = ba.BankAccountId,
                        Balance = ba.Balance
                    })
                .Where(ba => ba.UserId == userId)
                .ToList();


            var creditCards = context
                .CreditCards.Join(context.PaymentMethods,
                    (c => c.CreditCardId),
                    (p => p.CreditCardId),
                    (c, p) => new
                    {
                        UserId = p.UserId,
                        CreditCardId = c.CreditCardId,
                        LimitLeft = c.LimitLeft
                    })
                .Where(c => c.UserId == userId)
                .ToList();

            allАvailableMoneyOfUser += bankAccounts.Sum(b => b.Balance);
            allАvailableMoneyOfUser += creditCards.Sum(c => c.LimitLeft);

            if (allАvailableMoneyOfUser < amount)
            {
                throw new InvalidOperationException("Insufficient funds!");
            }

            bool isPayBills = false;
            foreach (var bankAccount in bankAccounts.OrderBy(b => b.BankAccountId))
            {
                var currentAccount = context.BankAccounts.Find(bankAccount.BankAccountId);

                if (amount <= currentAccount.Balance)
                {
                    currentAccount.Withdraw(amount);
                    isPayBills = true;
                }
                else
                {
                    amount -= currentAccount.Balance;
                    currentAccount.Withdraw(currentAccount.Balance);
                }

                if (isPayBills)
                {
                    context.SaveChanges();
                    return;
                }
            }

            foreach (var creditCard in creditCards.OrderBy(c => c.CreditCardId))
            {
                var currentCreditCard = context.CreditCards.Find(creditCard.CreditCardId);

                if (amount <= currentCreditCard.LimitLeft)
                {
                    currentCreditCard.Withdraw(amount);
                    isPayBills = true;
                }
                else
                {
                    amount -= currentCreditCard.LimitLeft;
                    currentCreditCard.Withdraw(currentCreditCard.LimitLeft);
                }

                if (isPayBills)
                {
                    context.SaveChanges();
                    return;
                }
            }
        }

        private static void UserDetailsById(BillsPaymentSystemContext context)
        {
            Console.Write("Enter userId: ");
            var userId = int.Parse(Console.ReadLine());
            var user = context.Users.Find(userId);

            if (user == null)
            {
                Console.WriteLine($"User with id {userId} not found!");
                return;
            }

            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
            var bankAccounts = context
                .BankAccounts.Join
                (context.PaymentMethods,
                    (b => b.BankAccountId),
                    (p => p.BankAccountId),
                    (b, p) => new
                    {
                        UserId = p.UserId,
                        BankAccountId = b.BankAccountId,
                        Balance = b.Balance,
                        BankName = b.BankName,
                        SwiftCode = b.SwiftCode
                    })
                    .Where(b => b.UserId == userId)
                    .ToList();

            Console.WriteLine("Bank Accounts:");
            foreach (var b in bankAccounts)
            {
                Console.WriteLine($"-- ID: {b.BankAccountId}");
                Console.WriteLine($"--- Balance: {b.Balance:F2}");
                Console.WriteLine($"--- Bank: {b.BankName}");
                Console.WriteLine($"--- SWIFT: {b.SwiftCode}");
            }

            var creditCards = context
                .PaymentMethods
                .Join(context.CreditCards,
                    (p => p.CreditCardId),
                    (c => c.CreditCardId),
                    (p, c) => new
                    {
                        UserId = p.UserId,
                        CreditCardId = c.CreditCardId,
                        Limit = c.Limit,
                        MoneyOwned = c.MoneyOwed,
                        LimitLeft = c.LimitLeft,
                        ExpirationDate = c.ExpirationDate
                    })
                    .Where(b => b.UserId == userId)
                    .ToList();

            Console.WriteLine("Credit Cards:");
            foreach (var c in creditCards)
            {
                Console.WriteLine($"-- ID: {c.CreditCardId}");
                Console.WriteLine($"--- Limit: {c.Limit:F2}");
                Console.WriteLine($"--- Money Owed: {c.MoneyOwned:F2}");
                Console.WriteLine($"--- Limit Left:: {c.LimitLeft:F2}");
                Console.WriteLine($"--- Expiration Date: {c.ExpirationDate.Year}/{c.ExpirationDate.Month}");
            }
        }

        private static void Seed(BillsPaymentSystemContext context)
        {
            var firstAcc = new BankAccount
            {
                BankName = "First Investment Bank",
                Balance = 1000,
                SwiftCode = "FINVBGSF",
            };

            var secondAcc = new BankAccount
            {
                BankName = "Unicredit Bulbank",
                Balance = 5000,
                SwiftCode = "UNCRBGSF"
            };

            context
                .BankAccounts
                .AddRange(firstAcc, secondAcc);

            context.SaveChanges();

            var firstCreditCard = new CreditCard
            {
                Limit = 1000,
                MoneyOwed = 200,
                ExpirationDate = new DateTime(2020, 01, 01)
            };

            var secondCreditCard = new CreditCard
            {
                Limit = 2000,
                MoneyOwed = 500,
                ExpirationDate = new DateTime(2020, 01, 01)
            };

            context
                .CreditCards
                .AddRange(firstCreditCard, secondCreditCard);

            context.SaveChanges();

            var ivan = new User
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan@abv.bg",
                Password = "123456"
            };

            var pesho = new User
            {
                FirstName = "Petur",
                LastName = "Petrov",
                Email = "petur@abv.bg",
                Password = "123456"
            };

            context
                .Users
                .AddRange(ivan, pesho);

            context.SaveChanges();

            var firstPayment = new PaymentMethod
            {
                BankAccount = firstAcc,
                User = ivan,
                PaymentType = PaymentType.BankAccount,
            };

            var secondPayment = new PaymentMethod
            {
                BankAccount = secondAcc,
                User = pesho,
                PaymentType = PaymentType.BankAccount
            };

            var thirdPayment = new PaymentMethod
            {
                CreditCard = firstCreditCard,
                User = pesho,
                PaymentType = PaymentType.CreditCard
            };

            var fourthPayment = new PaymentMethod
            {
                CreditCard = secondCreditCard,
                User = ivan,
                PaymentType = PaymentType.CreditCard
            };

            context
                .PaymentMethods
                .AddRange(firstPayment, secondPayment, thirdPayment, fourthPayment);
            context.SaveChanges();
        }
    }
}
