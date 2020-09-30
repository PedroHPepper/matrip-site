using Matrip.Domain.Libraries.Text;
using Matrip.Domain.Models.Entities;
using Matrip.Domain.Models.TripPurchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Matrip.Domain.Libraries.Operations
{
    public class CalculateValues
    {
        /// <summary>
        /// Calcula o valor total em um carrinho de compras
        /// </summary>
        /// <param name="tripitemshoppingcartList">Carrinho de compras</param>
        /// <returns></returns>
        public static double GetCalculatedTotalValue(List<ma18tripitemshoppingcart> tripitemshoppingcartList)
        {
            double TotalValue = 0;
            foreach (ma18tripitemshoppingcart tripitemshoppingcart in tripitemshoppingcartList)
            {
                foreach (ma19SubTripItemShoppingCart subTripItemShoppingCart in tripitemshoppingcart.ma19SubTripItemShoppingCart)
                {
                    //Resgata o valor de item do subpasseio. Se tem desconto ele é feito em cima do valor original.
                    double value = subTripItemShoppingCart.ma17SubtripValue.ma17value;
                    if (subTripItemShoppingCart.ma14subtrip.ma14PartnerDiscountPercent > 0
                        && DateConvert.HrBrasilia() >= subTripItemShoppingCart.ma14subtrip.ma14InitialDiscountDate
                        && DateConvert.HrBrasilia() <= subTripItemShoppingCart.ma14subtrip.ma14FinalDiscountDate)
                    {
                        value = CalculateValues.CalculateValueWithDiscount(value, subTripItemShoppingCart.ma14subtrip.ma14PartnerDiscountPercent);
                    }
                    //percorre os valores de serviços e adiciona os valores em cima da quantidade total e valor da unidade
                    foreach (ma20ServiceItemShoppingCart serviceItemShoppingCart in subTripItemShoppingCart.ma20ServiceItemShoppingCart)
                    {
                        TotalValue += serviceItemShoppingCart.ma11service.ma11Value * serviceItemShoppingCart.ma20ServiceQuantity;
                    }
                    //verifica se o valor é por unidade é individual ou não. Se for, calcula em cima do desconto de cada turista, se não
                    //o valor será cheio.
                    if (subTripItemShoppingCart.ma17SubtripValue.ma17type == "0")
                    {
                        foreach (ma29TouristShoppingCart ma29TouristShoppingCart in tripitemshoppingcart.ma29TouristShoppingCart)
                        {
                            TotalValue += CalculateTouristValue(value, ma29TouristShoppingCart.ma27AgeDiscount.ma27DiscountPercent);
                        }
                    }
                    else
                    {
                        TotalValue += value;
                    }
                }
            }
            return TotalValue;
        }
        /// <summary>
        /// Calcula ou não o valor total de um carrinho de compras com desconto.
        /// </summary>
        /// <param name="tripitemshoppingcartList">Carrinho de compras</param>
        /// <param name="discount">Desconto</param>
        /// <returns></returns>
        public static double GetCalculatedTotalValue(List<ma18tripitemshoppingcart> tripitemshoppingcartList, float? discount)
        {
            double TotalValue = 0;
            foreach (ma18tripitemshoppingcart tripitemshoppingcart in tripitemshoppingcartList)
            {
                foreach (ma19SubTripItemShoppingCart subTripItemShoppingCart in tripitemshoppingcart.ma19SubTripItemShoppingCart)
                {
                    foreach (ma20ServiceItemShoppingCart serviceItemShoppingCart in subTripItemShoppingCart.ma20ServiceItemShoppingCart)
                    {
                        TotalValue += serviceItemShoppingCart.ma11service.ma11Value * serviceItemShoppingCart.ma20ServiceQuantity;
                    }

                    double value = subTripItemShoppingCart.ma17SubtripValue.ma17value;
                    double totalDiscount = 0;
                    if (discount != null && subTripItemShoppingCart.ma14subtrip.ma14InfluencerDiscount)
                    {
                        totalDiscount += discount.Value;
                    }
                    if (subTripItemShoppingCart.ma14subtrip.ma14PartnerDiscountPercent > 0
                        && DateConvert.HrBrasilia() >= subTripItemShoppingCart.ma14subtrip.ma14InitialDiscountDate
                        && DateConvert.HrBrasilia() <= subTripItemShoppingCart.ma14subtrip.ma14FinalDiscountDate)
                    {
                        totalDiscount += subTripItemShoppingCart.ma14subtrip.ma14PartnerDiscountPercent;
                    }
                    value = value - (value * (totalDiscount / 100));
                    if (subTripItemShoppingCart.ma17SubtripValue.ma17type == "0")
                    {
                        foreach (ma29TouristShoppingCart ma29TouristShoppingCart in tripitemshoppingcart.ma29TouristShoppingCart)
                        {
                            TotalValue += CalculateValues.CalculateTouristValue(value,
                                                            ma29TouristShoppingCart.ma27AgeDiscount.ma27DiscountPercent);
                        }
                    }
                    else
                    {
                        TotalValue += value;
                    }
                }
            }
            return TotalValue;
        }

        /// <summary>
        /// Calcula um item de passeio com ou sem desconto.
        /// </summary>
        /// <param name="choosedTripPackage">Item de Passeio</param>
        /// <param name="discount">Desconto</param>
        /// <returns></returns>
        public static double GetCalculatedTotalValue(ChoosedTripPackage choosedTripPackage, float? discount)
        {
            double TotalValue = 0;
            foreach (ma14subtrip ma14subtrip in choosedTripPackage.trip.ma14subtrip)
            {
                //percorre os valores de serviços e adiciona os valores em cima da quantidade total e valor da unidade
                foreach (ma11service ma11service in ma14subtrip.ma11service)
                {
                    TotalValue += ma11service.ma11Value * choosedTripPackage.TripItem.Services
                                                            .Where(e => e.ServiceID == ma11service.ma11idservice).FirstOrDefault().Quantity;
                }

                int choosedSubTrip = int.Parse(choosedTripPackage.TripItem.SubtripStatus.Split("/")[1]);
                if (ma14subtrip.ma14idsubtrip == choosedSubTrip)
                {
                    ma17SubtripValue subtripValue = ma14subtrip.ma17SubtripValue.Where(e => e.ma17idSubtripValue == int.Parse(choosedTripPackage.TripItem.SubtripStatus.Split("/")[2])).FirstOrDefault();
                    //Resgata o valor de item do subpasseio. Se tem desconto ele é feito em cima do valor original.
                    double value = subtripValue.ma17value;
                    float totalDiscount = 0;
                    if (discount != null && ma14subtrip.ma14InfluencerDiscount)
                    {
                        totalDiscount += discount.Value;
                    }
                    if (ma14subtrip.ma14PartnerDiscountPercent > 0
                        && DateConvert.HrBrasilia() >= ma14subtrip.ma14InitialDiscountDate
                        && DateConvert.HrBrasilia() <= ma14subtrip.ma14FinalDiscountDate)
                    {
                        totalDiscount += ma14subtrip.ma14PartnerDiscountPercent;
                    }
                    value = CalculateValues.CalculateValueWithDiscount(value, totalDiscount);
                    //verifica se o valor é por unidade é individual ou não. Se for, calcula em cima do desconto de cada turista, se não
                    //o valor será cheio.
                    if (subtripValue.ma17type == "0")
                    {
                        foreach (TouristModel tourist in choosedTripPackage.TouristList)
                        {
                            int discountPercent = choosedTripPackage.trip.ma27AgeDiscount
                                .Where(e => e.ma27idAgeDiscount == tourist.AgeDiscountID).FirstOrDefault().ma27DiscountPercent;

                            TotalValue += CalculateValues.CalculateTouristValue(value, discountPercent);
                        }
                    }
                    else
                    {
                        TotalValue += value;
                    }
                }
            }
            return TotalValue;
        }
        /// <summary>
        /// Calcula os descontos para um turista.
        /// </summary>
        /// <param name="value">Valor</param>
        /// <param name="discountPercent">Desconto em int.</param>
        /// <returns></returns>
        private static double CalculateTouristValue(double value, int discountPercent)
        {
            var discount = value * (Convert.ToDouble(discountPercent) / 100);
            return value - discount;
        }

        /// <summary>
        /// Calcula o desconto para o valor.
        /// </summary>
        /// <param name="value">Valor</param>
        /// <param name="discountPercent">Desconto em int</param>
        /// <returns></returns>
        private static double CalculateValueWithDiscount(double value, float discountPercent)
        {
            var discount = value * (Convert.ToDouble(discountPercent) / 100);
            return value - discount;
        }

        /// <summary>
        /// Calcula o valor total de uma venda.
        /// </summary>
        /// <param name="ma32sale">Venda completa</param>
        /// <returns></returns>
        public static double CalculateTotalSaleValue(ma32sale ma32sale)
        {
            double totalSaleValue = 0;
            foreach (ma21saleTrip ma21saleTrip in ma32sale.ma21saleTrip)
            {
                foreach (ma22subtripsale ma22subtripsale in ma21saleTrip.ma22subtripsale)
                {
                    totalSaleValue += ma22subtripsale.ma22saleValue;
                    foreach (ma23servicesale ma23servicesale in ma22subtripsale.ma23servicesale)
                    {
                        totalSaleValue += ma23servicesale.ma23TotalValue;
                    }
                }
            }
            return totalSaleValue;
        }
    }
}
