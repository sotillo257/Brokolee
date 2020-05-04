using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Concrete;
using WebApplication1.Models;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class cuotasController : Controller
    {
        pjrdev_condominiosEntities entities = new pjrdev_condominiosEntities();
        EFPublicRepository ep = new EFPublicRepository();
        List<Titulo> titulosList = new List<Titulo>();
        List<community> listComunities = new List<community>();

        // GET: concepto
        public ActionResult balance()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }

                    long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);

                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    conceptoViewModel viewModel = new conceptoViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;
                                    
                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_balance";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                   
                    long adminId = (long)curUser.create_userid;
                    List<fee> feeList = new List<fee>();
                    if (Session["CURRENT_COMU"] != null)
                    {
                        feeList = entities.fees.Where(m => m.user_id == adminId && m.community_id == communityAct).ToList();
                    }
                    else
                    {
                        feeList.Clear();
                    }                    
                    viewModel.feeList = feeList;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }                            
            }
            else
            {
                return Redirect(Url.Action("Index", "iniciar"));
            }

        }

        public ActionResult estado(string searchString = "", string searchState = "")
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }
                    decimal totalCredit = 0;
                    decimal totalDebit = 0;
                    decimal curBalance = 0;
                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    List<transaction> transactionList = new List<transaction>();

                    if (searchString == "" && searchState == "")
                    {
                        transactionList = entities.transactions.ToList();
                    }
                    else if (searchString != "" && searchState == "")
                    {
                        transactionList = entities.transactions.Where(m => m.kind.Contains(searchString) == true).ToList();
                    }
                    else if (searchString == "" && searchState != "")
                    {
                        transactionList = entities.transactions.Where(m => m.state.Contains(searchState) == true).ToList();
                    }
                    else
                    {
                        transactionList = entities.transactions.Where(m => m.kind.Contains(searchString) == true &&
                                          m.state.Contains(searchState) == true).ToList();
                    }

                    transaction lastTransaction = entities.transactions.OrderBy(m => m.date_info).FirstOrDefault();
                    if (lastTransaction != null)
                    {
                        curBalance = (decimal)lastTransaction.balance;
                    }

                    foreach (var item in transactionList)
                    {
                        if (item.debit != null)
                        {
                            totalDebit += (decimal)item.debit;
                        }

                        if (item.credit != null)
                        {
                            totalCredit += (decimal)item.credit;
                        }
                    }
                    estadoCuotasViewModel viewModel = new estadoCuotasViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;

                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_estado";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);               
                    viewModel.transactionList = transactionList;
                    viewModel.totalCredit = totalCredit;
                    viewModel.totalDebit = totalDebit;
                    viewModel.curBalance = curBalance;
                    return View(viewModel);
                }
                catch(Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }                
            }
            else
            {
                return Redirect(Url.Action("Index", "iniciar"));
            }
        }

        public ActionResult agregar()
        {
            if (Session["USER_ID"] != null)
            {
                try
                {
                    long userId = 0;
                    if (Convert.ToInt32(Session["USER_ROLE"]) == 1)
                    {
                        userId = (long)Session["USER_ID"];
                    }
                    else if (Convert.ToInt32(Session["USER_ROLE"]) > 1
                    && Session["ACC_USER_ID"] != null)
                    {
                        userId = (long)Session["ACC_USER_ID"];
                    }

                    user curUser = entities.users.Find(userId);
                    List<ShowMessage> pubMessageList = ep.GetChatMessages(userId);
                    conceptoViewModel viewModel = new conceptoViewModel();

                    titulosList = ep.GetTitulosByTitular(userId);
                    listComunities = ep.GetCommunityListByTitular(titulosList);
                    viewModel.communityList = listComunities;

                    viewModel.side_menu = "cuotas";
                    viewModel.side_sub_menu = "cuotas_agregar";
                    viewModel.document_category_list = entities.document_type.ToList();
                    viewModel.curUser = curUser;
                    viewModel.pubTaskList = ep.GetNotifiTaskList(userId);
                    viewModel.pubMessageList = pubMessageList;
                    viewModel.messageCount = ep.GetUnreadMessageCount(pubMessageList);                  
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    return Redirect(Url.Action("Index", "Error"));
                }               
            }
            else
            {
                return Redirect(Url.Action("Index", "iniciar"));
            }

        }

        [HttpPost]
        public async Task<ActionResult> MakePayment(decimal cost, decimal taxCharge, decimal penalty)
        {
            try
            {
                long communityAct = Convert.ToInt64(Session["CURRENT_COMU"]);
                string paymentType = "CC"; // CreditCard
                bool makePayment = false;
                long userId = (long)Session["USER_ID"];
                user curUser = entities.users.Find(userId);
                long adminId = (long)curUser.create_userid;
                fee feeItem = entities.fees.Where(m => m.user_id == adminId && m.community_id == communityAct).FirstOrDefault();
                bank adminBankItem = entities.banks.Find(feeItem.bank_id);
                string bank_account = adminBankItem.account_number;
                string routing_number = adminBankItem.route_number;
                string account_name = adminBankItem.account_name;
                user adminUser = entities.users.Find(adminId);
                decimal totalAmount = cost + taxCharge + penalty;
                creditcard creditcardItem = entities.creditcards.Where(m => m.user_id == userId).FirstOrDefault();
                StripeConfiguration.SetApiKey(ep.GetStripeSecretKey());

                if (creditcardItem != null)
                {
                    DateTime expire_date = (DateTime)creditcardItem.expiration_date;
                    var options = new TokenCreateOptions
                    {
                        Card = new CreditCardOptions
                        {
                            Number = creditcardItem.card_number, //"4000 0000 0000 0077", //creditcardItem.card_number,//"4242424242424242"
                            ExpYear = expire_date.Year,//2020
                            ExpMonth = expire_date.Month,
                            Cvc = creditcardItem.ccv
                        }
                    };
                    var service = new TokenService();
                    Token stripeToken = service.Create(options);
                    int pc = await PaymentCredit(stripeToken.Id, Convert.ToInt32(cost * 100),
                        bank_account, routing_number, account_name);
                    if (pc == 1)
                    {
                        paymentType = "CC";
                        makePayment = true;
                    }
                }
                else
                {
                    bank userBank = entities.banks.Where(m => m.user_id == userId).FirstOrDefault();
                    if (userBank != null)
                    {
                        var options = new TokenCreateOptions
                        {
                            BankAccount = new BankAccountOptions
                            {
                                Country = "US",
                                Currency = "usd",
                                AccountHolderName = userBank.account_name,//"Jenny Rosen"
                                AccountHolderType = "individual",
                                RoutingNumber = userBank.route_number,//"110000000"
                                AccountNumber = userBank.account_number//"000123456789"
                            }
                        };
                        var service = new TokenService();
                        Token stripeToken = service.Create(options);
                        int ach = await PaymentAch(stripeToken.Id, Convert.ToInt32(totalAmount * 100),
                            bank_account, routing_number);

                        if (ach == 1)
                        {
                            paymentType = "ACH";
                            makePayment = true;
                        }
                    }
                }

                if (makePayment == true)
                {
                    transaction transItem = new transaction();

                    if (transItem.balance != null)
                    {
                        decimal originBalance = (decimal)transItem.balance;
                        transItem.balance = originBalance + totalAmount;
                    }
                    else
                    {
                        transItem.balance = totalAmount;
                    }
                    transItem.credit = 0;
                    transItem.debit = totalAmount;
                    transItem.date_info = DateTime.Now;
                    transItem.kind = "Factura";
                    transItem.state = "Factura";
                    transItem.trans_id = "";
                    entities.transactions.Add(transItem);

                    payment paymentItem = new payment();
                    paymentItem.method = paymentType;
                    paymentItem.date_issue = DateTime.Now;
                    paymentItem.date_payment = null;
                    paymentItem.email = adminUser.email;
                    paymentItem.first_name = adminUser.first_name1;
                    paymentItem.surname = adminUser.last_name1;
                    paymentItem.to_user_id = adminId;
                    paymentItem.quantity = totalAmount;
                    paymentItem.user_id = userId;
                    paymentItem.state = true;
                    entities.payments.Add(paymentItem);
                    entities.SaveChanges();
                }

                return Redirect(Url.Action("estado", "cuotas"));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("balance", "cuotas"));
            }
        }

        public async Task<int> PaymentAch(string bankToken, int amount,
            string account_number, string rounting_number)
        {
            try
            {
                // Create an ACH charge
                StripeConfiguration.SetApiKey(ep.GetStripeSecretKey());
                CustomerService customerService = new CustomerService();
                var customerOptions = new CustomerCreateOptions
                {

                };
                Customer achCustomer = customerService.Create(customerOptions);

                // Create Bank Account
                var bankAccountOptions = new BankAccountCreateOptions
                {
                    SourceToken = bankToken
                };
                var bankService = new BankAccountService();
                BankAccount bankAccount = bankService.Create(achCustomer.Id, bankAccountOptions);
                // Verify BankAccount
                List<long> Amounts = new List<long>();
                Amounts.Add(32);
                Amounts.Add(45);

                var verifyOptions = new BankAccountVerifyOptions
                {
                    Amounts = Amounts
                };

                bankAccount = bankService.Verify(achCustomer.Id, bankAccount.Id, verifyOptions);
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Description = "Charge for jenny.rosen@example.com",
                    CustomerId = achCustomer.Id
                };
                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeOptions);
                // Payout from Stripe to Bank Account
                string bankAccountStr = account_number;// "000123456789";
                string bankRoutingStr = rounting_number; // "110000000";

                // Get self account
                var accountOptions = new AccountCreateOptions
                {
                    Email = "D.gonzalez@zerosystempr.com",
                    Type = AccountType.Custom,
                    Country = "US",
                    ExternalBankAccount = new AccountBankAccountOptions()
                    {
                        Country = "US",
                        Currency = "usd",
                        AccountHolderName = "John Brown",//account_name
                        AccountHolderType = "individual",
                        RoutingNumber = bankRoutingStr,
                        AccountNumber = bankAccountStr
                    },
                    PayoutSchedule = new AccountPayoutScheduleOptions()
                    {
                        Interval = "daily"
                    },
                    TosAcceptance = new AccountTosAcceptanceOptions()
                    {
                        Date = DateTime.Now.AddDays(-10),
                        Ip = "202.47.115.80",
                        UserAgent = "Chrome"
                    },
                    LegalEntity = new AccountLegalEntityOptions()
                    {
                        Dob = new AccountDobOptions()
                        {
                            Day = 1,
                            Month = 4,
                            Year = 1991
                        },
                        FirstName = "John",
                        LastName = "Brown",
                        Type = "individual"
                    }
                };
                var accountService = new AccountService();
                Account account = await accountService.CreateAsync(accountOptions);
                StripeConfiguration.SetApiKey(ep.GetStripeSecretKey());

                var service = new BalanceService();
                Balance balance = await service.GetAsync();

                var transOptions = new TransferCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Destination = account.Id
                };

                var transferService = new TransferService();
                Transfer Transfer = transferService.Create(transOptions);

                var payoutOptions = new PayoutCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Destination = account.ExternalAccounts.First().Id,
                    SourceType = "card",
                    StatementDescriptor = "PAYOUT",
                    //Method = "instant"
                };

                var requestOptions = new RequestOptions();
                requestOptions.ApiKey = ep.GetStripeSecretKey();
                requestOptions.StripeConnectAccountId = account.Id;
                var payoutService = new PayoutService();
                var payout = await payoutService.CreateAsync(payoutOptions, requestOptions);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> PaymentCredit(string token, int amount,
            string bank_account, string routing_number, string account_name)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                user residentUser = entities.users.Find(userId);
                // Charge using Credit Card
                StripeConfiguration.SetApiKey(ep.GetStripeSecretKey());
                var chargeOptions = new ChargeCreateOptions
                {
                    Amount = amount + 20000,
                    Currency = "usd",
                    Description = "Charge for jenny.rosen@example.com",
                    SourceId = token // obtained with Stripe.js,                    
                };
                var chargeService = new ChargeService();
                Charge charge = chargeService.Create(chargeOptions);
                // Payout to coamdin bank
                // Get Bank Info
                string bankAccountStr = bank_account;//"000123456789";//
                string bankRoutingStr = routing_number;// "110000000";//

                // Get self account
                var accountOptions = new AccountCreateOptions
                {
                    Email = "D.gonzalez@zerosystempr.com",
                    Type = AccountType.Custom,
                    Country = "US",
                    ExternalBankAccount = new AccountBankAccountOptions()
                    {
                        Country = "US",
                        Currency = "usd",
                        AccountHolderName = "John Brown",//account_name
                        AccountHolderType = "individual",
                        RoutingNumber = bankRoutingStr,
                        AccountNumber = bankAccountStr
                    },
                    PayoutSchedule = new AccountPayoutScheduleOptions()
                    {
                        Interval = "daily"
                    },
                    TosAcceptance = new AccountTosAcceptanceOptions()
                    {
                        Date = DateTime.Now.AddDays(-10),
                        Ip = "202.47.115.80",
                        UserAgent = "Chrome"
                    },
                    LegalEntity = new AccountLegalEntityOptions()
                    {
                        Dob = new AccountDobOptions()
                        {
                            Day = 1,
                            Month = 4,
                            Year = 1991
                        },
                        FirstName = "John",
                        LastName = "Brown",
                        Type = "individual"
                    }
                };
                var accountService = new AccountService();
                Account account = await accountService.CreateAsync(accountOptions);
                StripeConfiguration.SetApiKey(ep.GetStripeSecretKey());

                var service = new BalanceService();
                Balance balance = await service.GetAsync();

                var transOptions = new TransferCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Destination = account.Id
                };

                var transferService = new TransferService();
                Transfer Transfer = transferService.Create(transOptions);

                var payoutOptions = new PayoutCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    Destination = account.ExternalAccounts.First().Id,
                    SourceType = "card",
                    StatementDescriptor = "PAYOUT",
                    //Method = "instant"
                };

                var requestOptions = new RequestOptions();
                requestOptions.ApiKey = ep.GetStripeSecretKey();
                requestOptions.StripeConnectAccountId = account.Id;
                var payoutService = new PayoutService(); 
                var payout = await payoutService.CreateAsync(payoutOptions, requestOptions);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }


        [HttpPost]
        public ActionResult addcredit(string first_name, string surname, string card_number,
            int month, int year, string ccv, string address, string country, string state, string city,
            string postal_code, string phone)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                creditcard creditcardItem = entities.creditcards.Where(m => m.user_id == userId).FirstOrDefault();

                if (creditcardItem == null)
                {
                    creditcardItem = new creditcard();
                    creditcardItem.first_name = first_name;
                    creditcardItem.surname = surname;
                    creditcardItem.card_number = card_number;
                    creditcardItem.ccv = ccv;
                    creditcardItem.address = address;
                    creditcardItem.country = country;
                    creditcardItem.state = state;
                    creditcardItem.city = city;
                    creditcardItem.postal_code = postal_code;
                    creditcardItem.phone = phone;
                    creditcardItem.user_id = userId;
                    creditcardItem.expiration_date = new DateTime(year, month, 1);
                    creditcardItem.created_at = DateTime.Now;
                    creditcardItem.updated_at = DateTime.Now;
                    entities.creditcards.Add(creditcardItem);
                }
                else
                {
                    creditcardItem.first_name = first_name;
                    creditcardItem.surname = surname;
                    creditcardItem.card_number = card_number;
                    creditcardItem.ccv = ccv;
                    creditcardItem.address = address;
                    creditcardItem.country = country;
                    creditcardItem.state = state;
                    creditcardItem.city = city;
                    creditcardItem.postal_code = postal_code;
                    creditcardItem.phone = phone;
                    creditcardItem.expiration_date = new DateTime(year, month, 1);
                    creditcardItem.updated_at = DateTime.Now;
                }

                entities.SaveChanges();
                return Redirect(Url.Action("balance", "cuotas"));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("balance", "cuotas"));
            }
        }

        [HttpPost]
        public ActionResult addbank(string account_name, string account_number,
            string bank_name, string route_number)
        {
            try
            {
                long userId = (long)Session["USER_ID"];
                bank bankItem = entities.banks.Where(m => m.user_id == userId).FirstOrDefault();

                if (bankItem == null)
                {
                    bankItem = new bank();
                    bankItem.created_at = DateTime.Now;
                    bankItem.route_number = route_number;
                    bankItem.updated_at = DateTime.Now;
                    bankItem.account_name = account_name;
                    bankItem.account_number = account_number;
                    bankItem.bank_name = bank_name;
                    bankItem.user_id = userId;
                    entities.banks.Add(bankItem);
                }
                else
                {
                    bankItem.route_number = route_number;
                    bankItem.updated_at = DateTime.Now;
                    bankItem.account_name = account_name;
                    bankItem.account_number = account_number;
                    bankItem.bank_name = bank_name;
                }

                entities.SaveChanges();
                return Redirect(Url.Action("balance", "cuotas"));
            }
            catch(Exception ex)
            {
                return Redirect(Url.Action("balance", "cuotas"));
            }            
        }

        [HttpPost]
        public ActionResult SeacrhResult(string searchString, string searchState)
        {
            return Redirect(Url.Action("estado", "cuotas", new { searchString = searchString, searchState = searchState }));
        }
    }

}