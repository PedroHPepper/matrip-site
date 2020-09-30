using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Libraries.Validation;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.HomeModels;
using Matrip.Domain.Models.SaleModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Matrip.Domain.Libraries.Email
{
    /// <summary>
    /// Classe responsável por mandar e-mails através do email registrado no AppSettings.
    /// </summary>
    public class EmailManagement
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;

        public EmailManagement(SmtpClient smtp, IConfiguration configuration)
        {
            _smtp = smtp;
            _configuration = configuration;
        }
        /// <summary>
        /// Envia um email de boas vindas ao cliente.
        /// </summary>
        /// <param name="CustomerEmail">Email do cliente.</param>
        public void SendGreetingsEmail(string CustomerEmail)
        {
            if (EmailValidation.IsValidEmail(CustomerEmail))
            {
                string bodyMessage = string.Format("<h2>Bem Vindo(a) ao MATRIP</h2>" +
                "<b>Confira os melhores passeios turísticos em nosso <a href=\"{0}\">SITE</a>. </b> <br />" +
                "<br /> Email enviado automaticamente do site Matrip"
                , "https://www.matrip.com.br");

                /*
                 * MailMessage -> Build the message 
                 */
                MailMessage Message = new MailMessage();
                Message.From = new MailAddress("contatomatrip@gmail.com");
                Message.To.Add(CustomerEmail);
                Message.Subject = ">Bem Vindo(a) ao MATRIP";
                Message.Body = bodyMessage;
                Message.IsBodyHtml = true;

                try
                {
                    _smtp.Send(Message);
                }
                catch (Exception)
                {
                    throw new Exception("Email inválido");
                }
            }
            else
            {
                throw new Exception("Email inválido");
            }
        }
        /// <summary>
        /// Envia um link de confirmação de email para o cliente.
        /// </summary>
        /// <param name="callbackURL">Url de confirmação.</param>
        /// <param name="CustomerEmail">Email do Cliente.</param>
        public void SendEmailConfirmation(string callbackURL, string CustomerEmail)
        {
            string bodyMessage = string.Format("<h2>Confirmação de Email - MATRIP</h2>" +
                "<b>Para confirmar o seu Email, favor clique no link:</b> <a href=\"" + callbackURL + "\"> Confirmar Email </a> <br />" +
                "<br /> Email enviado automaticamente do site Matrip"
                );

            /*
             * MailMessage -> Build the message 
             */
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("contatomatrip@gmail.com");
            Message.To.Add(CustomerEmail);
            Message.Subject = "Confirmação de Email Matrip";
            Message.Body = bodyMessage;
            Message.IsBodyHtml = true;

            _smtp.Send(Message);
        }
        /// <summary>
        /// Envia um email para o próprio email da Matrip, contendo as mensagens enviadas de sugestões ou reclamações pela View de Contact.
        /// </summary>
        /// <param name="contact">Parâmetros de contato: Email, Assunto, Nome e Texto.</param>
        public void SendContactEmail(Contact contact)
        {
            string bodyMessage = string.Format(
                "<h2>Contato - Matrip</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>E-mail: </b> {1} <br />" +
                "<b>Assunto: </b> {2} <br />" +
                "<b>Texto: </b> {3} <br />" +
                "<br /> E-mail enviado automaticamente do site MATRIP.",
                contact.Name,
                contact.Email,
                contact.About,
                contact.Text
                );

            /*
             * MailMessage -> Build the message 
             */
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("contatomatrip@gmail.com");
            Message.To.Add("contatomatrip@gmail.com");
            Message.Subject = "Contato Matrip Email: " + contact.Email;
            Message.Body = bodyMessage;
            Message.IsBodyHtml = true;

            _smtp.Send(Message);
        }
        /// <summary>
        /// Envia um email com link para o usuário que o leva à uma troca de senha, quando este se esqueceu da senha da conta.
        /// </summary>
        /// <param name="callbackURL">Link que é enviado para o email do úsuário que o leva de volta ao site.</param>
        /// <param name="CustomerEmail">Email do cliente.</param>
        public void SendEmailResetPassword(string callbackURL, string CustomerEmail)
        {
            string bodyMessage = string.Format("<h2>Confirmação de Email - MATRIP</h2>" +
                "<b>Para troca de senha, clique no link:</b> <br /><a href=\"" + callbackURL + "\"> Trocar Senha </a> <br />" +
                "<br /> Email enviado automaticamente do site Matrip! <br />Após recuperar a senha realize uma troca de senha no seu perfil");

            /*
             * MailMessage -> Build the message 
             */
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("contatomatrip@gmail.com");
            Message.To.Add(CustomerEmail);
            Message.Subject = "Confirmação de Email Matrip";
            Message.Body = bodyMessage;
            Message.IsBodyHtml = true;

            _smtp.Send(Message);
        }
        /// <summary>
        /// Envia um email à matrip sobre transferencia pendente.
        /// </summary>
        /// <param name="user">Usuário que comprou um passeio.</param>
        public void SendTransferenceInformationEmail(ma01user user)
        {
            string bodyMessage = string.Format(
                "<h2>Transferência Pendente de Usuário - Matrip</h2>" +
                "<b>Nome: </b> {0} <br />" +
                "<b>E-mail: </b> {1} <br />" +
                "<b>ID do Usuário: </b> {2} <br />" +
                "<br /> E-mail enviado automaticamente do site MATRIP.",
                user.ma01FullName,
                user.Email,
                user.Id
                );

            /*
             * MailMessage -> Build the message 
             */
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("contatomatrip@gmail.com");
            Message.To.Add("contatomatrip@gmail.com");
            Message.Subject = "Transferência Pendente de Usuário - Matrip Email: " + user.Email;
            Message.Body = bodyMessage;
            Message.IsBodyHtml = true;

            _smtp.Send(Message);
        }
        /// <summary>
        /// Envia um link para o email do cliente que o leva ao link do contrato da compra.
        /// </summary>
        /// <param name="callbackURL">Link que o leva de volta ao site.</param>
        /// <param name="CustomerEmail">Email do cliente.</param>
        public void SendContractEmail(string callbackURL, string CustomerEmail)
        {
            string bodyMessage = string.Format("<h2>Envio de Contrato de Compra e Venda de Passeio - MATRIP</h2>" +
                "<b>Para visualizar seu contrato, clique no link:</b> <br /><a href=\"" + callbackURL + "\"> Visualizar Contrato </a> <br />" +
                "<br /> Email enviado automaticamente do site Matrip! <br />");

            /*
             * MailMessage -> Build the message 
             */
            MailMessage Message = new MailMessage();
            Message.From = new MailAddress("contatomatrip@gmail.com");
            Message.To.Add(CustomerEmail);
            Message.Subject = "Contrato de Compra e Venda Matrip";
            Message.Body = bodyMessage;
            Message.IsBodyHtml = true;

            _smtp.Send(Message);
        }

        /// <summary>
        /// Manda um email com os dados da venda para todos os parceiros envolvidos na venda.
        /// </summary>
        /// <param name="sale">Modelo completo da venda com dados do cliente.</param>
        /// <param name="userName">Nome do cliente.</param>
        public void SendPartnerEmail(SaleViewModel sale, string userName)
        {
            List<PartnerSalesModel> PartnerSalesList = GetPartnerSalesList(sale);
            foreach(PartnerSalesModel PartnerSales in PartnerSalesList)
            {
                if (!string.IsNullOrEmpty(PartnerSales.ma25partner.ma25email) && EmailValidation.IsValidEmail(PartnerSales.ma25partner.ma25email))
                {
                    string bodyMessage = string.Format("<h2>Venda de Passeio - MATRIP</h2><br />" +
                                                    "Nome do cliente: {0}<br />" +
                                                    "CPF: {1}<br />" +
                                                    "RG: {2}<br />" +
                                                    "<table width='1000' style='border:1px solid'>" +
                                                       "<tr style='border:1px solid'>" +
                                                            "<td style='border:1px solid'><strong>Cidade</ strong></ td>" +
                                                            "<td style='border:1px solid'><strong>Nome do Passeio</ strong></ td>" +
                                                            "<td style='border:1px solid'><strong>Detalhes dos Turistas</ strong></ td>" +
                                                            "<td style='border:1px solid'><strong>Nome do Roteiro do Passeio</ strong></ td>" +
                                                            "<td style='border:1px solid'><strong>Serviços Adicionais</ strong></ td>" +
                                                            "<td style='border:1px solid'><strong>Valor Total</ strong></ td>" +
                                                        "</ tr>",
                                                    userName,
                                                    sale.userAddress.ma33CPF,
                                                    sale.userAddress.ma33documentNumber);
                    foreach (ma21saleTrip saleTrip in PartnerSales.ma21saleTripList)
                    {
                        bodyMessage = bodyMessage + string.Format("<tr style='border:1px solid'>" +
                                                                        "<td style='border:1px solid'>" +
                                                                            "{0}" +
                                                                        "</ td >" +
                                                                        "<td style='border: 1px solid'>" +
                                                                            "{1}" +
                                                                        "</ td>" +
                                                                        "<td width='200' style='border:1px solid'>",
                                        saleTrip.ma05trip.ma09city.ma09name + " - " + saleTrip.ma05trip.ma09city.ma08uf.ma08UFInitials,
                                        saleTrip.ma05trip.ma05name);
                        foreach (ma27AgeDiscount ageDiscount in saleTrip.ma05trip.ma27AgeDiscount)
                        {
                            if (ageDiscount.ma27DiscountPercent > 0)
                            {
                                bodyMessage = bodyMessage + string.Format("<br />" +
                                    "<label>" +
                                    "<strong>{0}:</ strong >" +
                                    " x{1} - {2}% de desconto</ label>",
                                    ageDiscount.ma27name,
                                    saleTrip.ma28SaleTourist.Where(e => e.FK2827idAgeDiscount == ageDiscount.ma27idAgeDiscount).Count(),
                                    ageDiscount.ma27DiscountPercent);
                            }
                            else
                            {
                                bodyMessage = bodyMessage + string.Format("<br />" +
                                    "<label>" +
                                    "<strong>{0}:</ strong >" +
                                    " x{1}</ label>",
                                    ageDiscount.ma27name,
                                    saleTrip.ma28SaleTourist.Where(e => e.FK2827idAgeDiscount == ageDiscount.ma27idAgeDiscount).Count());
                            }
                            foreach (ma28SaleTourist saleTourist in saleTrip.ma28SaleTourist
                                        .Where(e => e.FK2827idAgeDiscount == ageDiscount.ma27idAgeDiscount).ToList())
                            {
                                bodyMessage = bodyMessage + string.Format("<br />" +
                                                                            "<label>Nome do turista:" +
                                                                                "{0}<br />" +
                                                                                "Documento: {1}</ label >",
                                                                            saleTourist.ma28Name,
                                                                            saleTourist.ma28PassportOrRG);
                            }
                        }
                        bodyMessage = bodyMessage + "</ td>";

                        double tripValue = 0;
                        foreach (ma22subtripsale subtripsale in saleTrip.ma22subtripsale.ToList())
                        {
                            tripValue += subtripsale.ma22saleValue;

                            bodyMessage = bodyMessage + string.Format("<td style='border:1px solid'>" +
                                                                        "<p>{0}: {1} - {2} Hrs <br /></p>",
                                                                        subtripsale.ma14subtrip.ma14name,
                                                                        subtripsale.ma22Date.ToString("dd/MM/yyyy"),
                                                                        subtripsale.ma22Entry.ToString("hh\\:mm"));
                            if(subtripsale.ma17SubtripValue.ma17type == "0")
                            {
                                bodyMessage = bodyMessage + string.Format("{0} (Individual)",
                                                                            ValueConvert.ConvertToReal(subtripsale.ma22originalSubtripValue));
                            }
                            else if (subtripsale.ma17SubtripValue.ma17type == "1")
                            {
                                bodyMessage = bodyMessage + string.Format("{0} (Privativo)",
                                                                            ValueConvert.ConvertToReal(subtripsale.ma22originalSubtripValue));
                            }
                            else
                            {
                                bodyMessage = bodyMessage + string.Format("{0} (Pacote: {1} Pessoas)",
                                                                            ValueConvert.ConvertToReal(subtripsale.ma22originalSubtripValue),
                                                                            subtripsale.ma17SubtripValue.ma17quantity);
                            }
                            if(subtripsale.ma22subtripPartnerlDiscount > 0)
                            {
                                bodyMessage = bodyMessage + string.Format("<p>- {0}% de desconto</ p>",
                                                                            subtripsale.ma22subtripPartnerlDiscount);
                            }
                            if(subtripsale.ma22subtripInfluencerDiscount > 0)
                            {
                                bodyMessage = bodyMessage + string.Format("<p>- {0}% de desconto (Cupom)</ p>",
                                                                            subtripsale.ma22subtripInfluencerDiscount);
                            }
                            bodyMessage = bodyMessage + "</ td>" +
                                                        "<td width='200' style='border:1px solid'>";
                            if (subtripsale.ma23servicesale != null)
                            {
                                foreach (ma23servicesale ma23servicesale in subtripsale.ma23servicesale)
                                {
                                    tripValue += ma23servicesale.ma23TotalValue;
                                    bodyMessage = bodyMessage + string.Format("<p>{0} x{1}</ p>" +
                                                                              "<p>{2}</ p>",
                                                                                ma23servicesale.ma11service.ma11name,
                                                                                ma23servicesale.ma23ServiceQuantity,
                                                                                ValueConvert.ConvertToReal(ma23servicesale.ma23TotalValue));
                                }
                            }
                            bodyMessage = bodyMessage + string.Format("</ td>" +
                                                        "<td width='100' style='border:1px solid'>" +
                                                            "{0}" +
                                                        "</td>", ValueConvert.ConvertToReal(tripValue));
                        }
                        bodyMessage = bodyMessage + "</tr>";
                    }

                    bodyMessage = bodyMessage + "</ table>" +
                        "<br /> Email enviado automaticamente do site Matrip! <br />";
                    /*
                        * MailMessage -> Build the message 
                    */
                    MailMessage Message = new MailMessage();
                    Message.From = new MailAddress("contatomatrip@gmail.com");
                    Message.To.Add(PartnerSales.ma25partner.ma25email);
                    Message.To.Add("contatomatrip@gmail.com");
                    Message.Subject = "Venda Matrip";
                    Message.Body = bodyMessage;
                    Message.IsBodyHtml = true;

                    _smtp.Send(Message);
                
                }

            }
            /*
            foreach (ma21saleTrip saleTrip in sale.sale.ma21saleTrip.ToList())
            {
                string bodyMessage = string.Format("<h2>Venda de Passeio - MATRIP</h2><br />" +
                    "Nome do cliente: {0}<br />" +
                    "CPF: {1}<br />" +
                    "RG: {2}<br />",
                    userName,
                    sale.userAddress.ma33CPF,
                    sale.userAddress.ma33documentNumber);
                foreach (ma22subtripsale subtripsale in saleTrip.ma22subtripsale.ToList())
                {
                    if(subtripsale.ma14subtrip.ma25partner.ma25email != null && EmailValidation.IsValidEmail(subtripsale.ma14subtrip.ma25partner.ma25email))
                    {
                        bodyMessage = bodyMessage + string.Format("<br />Passeio: {0}<br /> no valor de {1}<br />Lista de serviços: <br />",
                        subtripsale.ma14subtrip.ma14name,
                        ValueConvert.ConvertToReal(subtripsale.ma22saleValue));

                        if (subtripsale.ma23servicesale != null)
                        {
                            foreach (ma23servicesale servicesale in subtripsale.ma23servicesale.ToList())
                            {
                                string bodyService = string.Format("{0} x{1}<br /> com valor total de {2}",
                                    servicesale.ma11service.ma11name,
                                    servicesale.ma23ServiceQuantity,
                                    servicesale.ma23TotalValue);
                                bodyMessage = bodyMessage + bodyService;
                            }
                        }


                        bodyMessage = bodyMessage + "<br /> Email enviado automaticamente do site Matrip! <br />";

                        /*
                         * MailMessage -> Build the message 

                        MailMessage Message = new MailMessage();
                        Message.From = new MailAddress("contatomatrip@gmail.com");
                        Message.To.Add(subtripsale.ma14subtrip.ma25partner.ma25email);
                        Message.Subject = "Venda Matrip";
                        Message.Body = bodyMessage;
                        Message.IsBodyHtml = true;

                        _smtp.Send(Message);
                    }

                }
            }
                    */
        }

        /// <summary>
        /// Separa uma lista focada em cada parceiro que aparece na lista de compras do usuário.
        /// </summary>
        /// <param name="sale">Tabela de vendas</param>
        /// <returns></returns>
        private List<PartnerSalesModel> GetPartnerSalesList(SaleViewModel sale)
        {
            List<PartnerSalesModel> PartnerSalesList = new List<PartnerSalesModel>();
            foreach (ma21saleTrip saleTrip in sale.sale.ma21saleTrip.ToList())
            {
                ma25partner ma25partner = saleTrip.ma22subtripsale.ToList().FirstOrDefault().ma14subtrip.ma25partner;
                if (PartnerSalesList.Where(e => e.ma25partner.ma25idpartner == ma25partner.ma25idpartner).Any())
                {
                    PartnerSalesList.Where(e => e.ma25partner.ma25idpartner == ma25partner.ma25idpartner).FirstOrDefault().ma21saleTripList.Add(saleTrip);
                }
                else
                {
                    PartnerSalesModel partnerSale = new PartnerSalesModel()
                    {
                        ma25partner = ma25partner
                    };
                    partnerSale.ma21saleTripList = new List<ma21saleTrip>();
                    partnerSale.ma21saleTripList.Add(saleTrip);
                    PartnerSalesList.Add(partnerSale);
                }
            }
            return PartnerSalesList;
        }
    }
}
