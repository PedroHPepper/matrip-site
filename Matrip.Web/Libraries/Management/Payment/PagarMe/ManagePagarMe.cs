using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.Payment;
using Matrip.Domain.Models.TripPurchase;
using Matrip.Web.Domain.Models;
using Matrip.Web.Libraries.Login;
using Matrip.Web.Libraries.Text;
using Microsoft.Extensions.Configuration;
using PagarMe;
using System;
using System.Collections.Generic;

namespace Matrip.Web.Libraries.Management.Payment.PagarMe
{
    public class ManagePagarMe
    {
        private IConfiguration _configuration;
        private UserLogin _userLogin;
        public ManagePagarMe(IConfiguration configuration, UserLogin userLogin)
        {
            _configuration = configuration;
            _userLogin = userLogin;

        }

        public Transaction GenerateCreditCardPayment(ma01user User, List<ma18tripitemshoppingcart> tripitemshoppingcartList, string Cpf, CreditCard CreditCard,
                                                ma33UserAddress address, Installments Installment)
        {
            try
            {
                PagarMeService.DefaultApiKey = _configuration.GetValue<string>("Payment:PagarMe:ApiKey");
                PagarMeService.DefaultEncryptionKey = _configuration.GetValue<string>("Payment:PagarMe:EncryptionKey");

                Transaction transaction = new Transaction();
                transaction.PaymentMethod = PaymentMethod.CreditCard;

                Card card = new Card();
                card.Number = Mask.Remove(CreditCard.CardNumber);
                card.HolderName = CreditCard.CardName;
                card.ExpirationDate = CreditCard.ExpirationMonth + CreditCard.ExpirationYear;
                card.Cvv = CreditCard.SecurityNumber;
                try
                {
                    card.Save();
                    if (!card.Valid)
                    {
                        throw new Exception("Cartão não é válido! Favor, verifique os dados inseridos.");
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Cartão não é válido! Favor, verifique os dados inseridos.");
                }
                //TODO - Valor total do serviço

                transaction.Card = new Card
                {
                    Id = card.Id
                };

                transaction.Customer = new Customer
                {
                    ExternalId = User.Id.ToString(),
                    Name = User.ma01FullName,// ma01user.ma01name,
                    Type = CustomerType.Individual,
                    Country = "br",
                    Email = User.Email,//ma01user.ma01email,
                    Documents = new[]
                        {
                        new Document {
                            //TODO  - Criar um campo de CPF
                            Type = DocumentType.Cpf,
                            Number = Mask.Remove(Cpf)
                    }
                },
                    PhoneNumbers = new string[]
                        {
                        "+55" + Mask.Remove(User.PhoneNumber)
                    },
                    Birthday = User.ma01birth.ToString("yyyy-MM-dd")
                };
                transaction.Billing = new Billing
                {
                    Name = User.ma01FullName,
                    Address = new Address()
                    {
                        Country = "br",
                        State = address.ma33State,
                        City = address.ma33City,
                        Neighborhood = address.ma33Neighborhood,
                        Street = address.ma33Street,
                        StreetNumber = address.ma33StreetNumber,
                        Zipcode = Mask.Remove(address.ma33Zipcode)
                    }
                };

                var Today = DateConvert.HrBrasilia();

                Item[] items = new Item[tripitemshoppingcartList.Count];
                for (int i = 0; i < tripitemshoppingcartList.Count; i++)
                {
                    ma18tripitemshoppingcart tripitemshoppingcart = tripitemshoppingcartList[i];
                    var itemA = new Item()
                    {
                        Id = tripitemshoppingcart.ma05trip.ma05idtrip.ToString(),
                        Title = tripitemshoppingcart.ma05trip.ma05name,
                        Quantity = 1,
                        Tangible = true,
                        UnitPrice = Mask.ConvertValuePagarMe(Convert.ToDecimal(GetTripTotalValue(tripitemshoppingcart)))
                    };
                    items[i] = itemA;
                }
                transaction.Item = items;
                transaction.Amount = Mask.ConvertValuePagarMe(Installment.Value);
                transaction.Installments = Installment.InstallmentsNumber;

                transaction.Save();
                if (transaction.Status == TransactionStatus.Refused)
                {
                    throw new Exception("Cartão recusado.");
                }
                return transaction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }
        public Transaction GenerateCreditCardPaymentWithTripItem(ma01user User, ChoosedTripPackage choosedTripPackage, string Cpf, CreditCard CreditCard,
                                                ma33UserAddress address, Installments Installment, double totalValue)
        {
            try
            {
                PagarMeService.DefaultApiKey = _configuration.GetValue<string>("Payment:PagarMe:ApiKey");
                PagarMeService.DefaultEncryptionKey = _configuration.GetValue<string>("Payment:PagarMe:EncryptionKey");

                Transaction transaction = new Transaction();
                transaction.PaymentMethod = PaymentMethod.CreditCard;

                Card card = new Card();
                card.Number = Mask.Remove(CreditCard.CardNumber);
                card.HolderName = CreditCard.CardName;
                card.ExpirationDate = CreditCard.ExpirationMonth + CreditCard.ExpirationYear;
                card.Cvv = CreditCard.SecurityNumber;
                try
                {
                    card.Save();
                    if (!card.Valid)
                    {
                        throw new Exception("Cartão não é válido! Favor, verifique os dados inseridos.");
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Cartão não é válido! Favor, verifique os dados inseridos.");
                }
                //TODO - Valor total do serviço

                transaction.Card = new Card
                {
                    Id = card.Id
                };

                transaction.Customer = new Customer
                {
                    ExternalId = User.Id.ToString(),
                    Name = User.ma01FullName,// ma01user.ma01name,
                    Type = CustomerType.Individual,
                    Country = "br",
                    Email = User.Email,//ma01user.ma01email,
                    Documents = new[]
                        {
                        new Document {
                            //TODO  - Criar um campo de CPF
                            Type = DocumentType.Cpf,
                            Number = Mask.Remove(Cpf)
                    }
                },
                    PhoneNumbers = new string[]
                        {
                        "+55" + Mask.Remove(User.PhoneNumber)
                    },
                    Birthday = User.ma01birth.ToString("yyyy-MM-dd")
                };
                transaction.Billing = new Billing
                {
                    Name = User.ma01FullName,
                    Address = new Address()
                    {
                        Country = "br",
                        State = address.ma33State,
                        City = address.ma33City,
                        Neighborhood = address.ma33Neighborhood,
                        Street = address.ma33Street,
                        StreetNumber = address.ma33StreetNumber,
                        Zipcode = Mask.Remove(address.ma33Zipcode)
                    }
                };

                var Today = DateConvert.HrBrasilia();

                Item[] items = new Item[1];
                var itemA = new Item()
                {
                    Id = choosedTripPackage.trip.ma05idtrip.ToString(),
                    Title = choosedTripPackage.trip.ma05name,
                    Quantity = 1,
                    Tangible = true,
                    UnitPrice = Mask.ConvertValuePagarMe(Convert.ToDecimal(totalValue))
                };
                items[0] = itemA;
                transaction.Item = items;
                transaction.Amount = Mask.ConvertValuePagarMe(Installment.Value);
                transaction.Installments = Installment.InstallmentsNumber;

                transaction.Save();
                if (transaction.Status == TransactionStatus.Refused)
                {
                    throw new Exception("Cartão recusado.");
                }
                return transaction;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


        }
        public List<Installments> CalculateInstallmentsPayment(decimal Value)
        {
            List<Installments> lista = new List<Installments>();

            int maxInstallments = _configuration.GetValue<int>("Payment:PagarMe:MaxInstallments");
            decimal Interest = _configuration.GetValue<decimal>("Payment:PagarMe:Interest");

            for (int i = 1; i <= maxInstallments; i++)
            {
                Installments Installment = new Installments();
                Installment.InstallmentsNumber = i;

                decimal valorDoJuros = Value * Interest / 100;
                if (i == 1)
                {
                    Installment.Value = Value;
                    Installment.ValueByInstallments = Installment.Value / Installment.InstallmentsNumber;
                    Installment.interest = false;
                }
                else
                {
                    Installment.Value = Value + (i * valorDoJuros);
                    Installment.ValueByInstallments = Installment.Value / Installment.InstallmentsNumber;
                    Installment.interest = true;
                }
                lista.Add(Installment);
            }

            return lista;
        }
        public double GetTripTotalValue(ma18tripitemshoppingcart tripitemshoppingcart)
        {
            double TotalValue = 0;
            foreach (ma19SubTripItemShoppingCart subTripItemShoppingCart in tripitemshoppingcart.ma19SubTripItemShoppingCart)
            {
                foreach (ma20ServiceItemShoppingCart serviceItemShoppingCart in subTripItemShoppingCart.ma20ServiceItemShoppingCart)
                {
                    TotalValue += serviceItemShoppingCart.ma11service.ma11Value;
                }
                if (subTripItemShoppingCart.ma17SubtripValue.ma17type == "0")
                {
                    foreach (ma29TouristShoppingCart ma29TouristShoppingCart in tripitemshoppingcart.ma29TouristShoppingCart)
                    {
                        TotalValue += subTripItemShoppingCart.ma17SubtripValue.ma17value - (subTripItemShoppingCart.ma17SubtripValue.ma17value * (ma29TouristShoppingCart.ma27AgeDiscount.ma27DiscountPercent / 100));
                    }
                }
                else
                {
                    TotalValue += subTripItemShoppingCart.ma17SubtripValue.ma17value;
                }
            }
            return TotalValue;
        }

        public Transaction GetTransaction(string transactionId)
        {

            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Payment:PagarMe:ApiKey");

            return PagarMeService.GetDefaultService().Transactions.Find(transactionId);
        }

        public Transaction Chargeback(string transactionId)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Payment:PagarMe:ApiKey");

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(transactionId);

            transaction.Refund();

            return transaction;
        }
    }
}
