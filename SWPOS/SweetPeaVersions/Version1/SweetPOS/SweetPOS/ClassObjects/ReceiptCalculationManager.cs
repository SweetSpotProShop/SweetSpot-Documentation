using System;
using System.Collections.Generic;

namespace SweetPOS.ClassObjects
{
    public class ReceiptCalculationManager
    {
        public Receipt CalculateNewReceiptTotals(Receipt receipt)
        {
            receipt.fltCostTotal = ReturnCostAmount(receipt.lstReceiptItem);
            receipt.fltCartTotal = ReturnTotalAmount(receipt.lstReceiptItem, receipt.intReceiptSubNumber);
            receipt.fltDiscountTotal = ReturnDiscountAmount(receipt.lstReceiptItem);
            receipt.fltTradeInTotal = ReturnTradeInAmount(receipt.lstReceiptItem);
            TaxManager TM = new TaxManager();
            receipt.fltGovernmentTaxTotal = TM.ReturnGovernmentTaxTotal(receipt.lstReceiptItemTax);
            receipt.fltProvincialTaxTotal = TM.ReturnProvincialTaxTotal(receipt.lstReceiptItemTax);
            return receipt;
        }

        private double ReturnTotalAmount(List<ReceiptItem> receiptItems, int receiptSubNumber)
        {
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var item in receiptItems)
            {
                double useAmount = item.fltItemPrice;
                if (receiptSubNumber > 1)
                {
                    useAmount = item.fltItemRefund;
                }
                //Checks if the sku is outside of the range for the trade in sku's
                if (!item.bitIsTradeIn)
                {
                    singleTotalAmount = item.intItemQuantity * useAmount;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            //Returns the total amount value of the cart
            return totalTotalAmount;
        }
        private double ReturnCostAmount(List<ReceiptItem> receiptItems)
        {
            double singleTotalAmount = 0;
            double totalTotalAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var item in receiptItems)
            {
                //Checks if the sku is outside of the range for the trade in sku's
                if (!item.bitIsTradeIn)
                {
                    singleTotalAmount = item.intItemQuantity * item.fltItemAverageCostAtSale;
                    totalTotalAmount += singleTotalAmount;
                }
            }
            //Returns the total amount value of the cart
            return totalTotalAmount;
        }
        private double ReturnDiscountAmount(List<ReceiptItem> receiptItems)
        {
            double singleDiscount = 0;
            double totalDiscount = 0;
            //Loops through the cart and pulls each item
            foreach (var item in receiptItems)
            {
                //Determines if the discount was a percentage or a number
                if (item.bitIsPercentageDiscount)
                {
                    //If the discount is a percentage
                    singleDiscount = item.intItemQuantity * (item.fltItemPrice * (item.fltItemDiscount / 100));
                }
                else
                {
                    //If the discount is a dollar amount
                    singleDiscount = item.intItemQuantity * item.fltItemDiscount;
                }
                totalDiscount += singleDiscount;
            }
            //Returns the total discount as a double going to two decimal places
            return Math.Round(totalDiscount, 2);
        }
        private double ReturnTradeInAmount(List<ReceiptItem> receiptItems)
        {
            double singleTradeInAmount = 0;
            double totalTradeinAmount = 0;
            //Loops through the cart and pulls each item
            foreach (var item in receiptItems)
            {
                //Checking the sku and seeing if it falls in the trade in range.
                //If it does, the item is a trade in
                if (item.bitIsTradeIn)
                {
                    //Adding the trade in value to the total trade in amount
                    singleTradeInAmount = item.intItemQuantity * item.fltItemPrice;
                    totalTradeinAmount += singleTradeInAmount;
                }
            }
            //Returns the total trade in amount for the cart
            return totalTradeinAmount;
        }

        public bool CheckForItemsInTransaction(object transaction)
        {
            bool itemsInTransaction = false;

            if (transaction is PurchaseOrder)
            {
                PurchaseOrder purchaseOrder = transaction as PurchaseOrder;
                itemsInTransaction = CheckItemInPurchaseOrder(purchaseOrder);
            }
            else if (transaction is Invoice)
            {
                Invoice invoice = transaction as Invoice;
                itemsInTransaction = CheckItemInInvoice(invoice);
            }
            else if (transaction is Receipt)
            {
                Receipt receipt = transaction as Receipt;
                itemsInTransaction = CheckItemInReceipt(receipt);
            }

            return itemsInTransaction;
        }

        private bool CheckItemInPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            bool itemsInTransaction = false;
            if (purchaseOrder.intTransactionTypeID == 5)
            {
                if (purchaseOrder.lstPurchaseOrderItem.Count > 0)
                {
                    itemsInTransaction = true;
                }
            }
            else if (purchaseOrder.intTransactionTypeID == 8)
            {
                foreach (PurchaseOrderItem poItem in purchaseOrder.lstPurchaseOrderItem)
                {
                    if (poItem.intReceivedQuantity > 0)
                    {
                        itemsInTransaction = true;
                    }
                }
            }
            return itemsInTransaction;
        }
        private bool CheckItemInInvoice(Invoice invoice)
        {
            bool itemsInTransaction = false;
            if (invoice.lstInvoiceItem.Count > 0)
            {
                itemsInTransaction = true;
            }
            return itemsInTransaction;
        }
        private bool CheckItemInReceipt(Receipt receipt)
        {
            bool itemsInTransaction = false;
            if (receipt.lstReceiptItem.Count > 0)
            {
                itemsInTransaction = true;
            }
            return itemsInTransaction;
        }
    }
}