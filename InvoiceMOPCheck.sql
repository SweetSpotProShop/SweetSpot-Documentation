--Dev Database
--Lists only the invoice numbers without a MOP
SELECT
CONCAT(tbl_invoice.invoiceNum, '-', tbl_invoice.invoiceSubNum) AS 'Invoice'
FROM tbl_invoice
WHERE NOT EXISTS(SELECT TOP 1 tbl_invoiceMOP.invoiceNum FROM tbl_invoiceMOP WHERE CONCAT(tbl_invoiceMOP.invoiceNum, '-', tbl_invoiceMOP.invoiceSubNum) = CONCAT(tbl_invoice.invoiceNum, '-', tbl_invoice.invoiceSubNum))
--Returns the count of how many invoices do not have a MOP
SELECT 
COUNT(CONCAT(tbl_invoice.invoiceNum, '-', tbl_invoice.invoiceSubNum)) AS 'Invoice'
FROM tbl_invoice
WHERE NOT EXISTS(SELECT TOP 1 tbl_invoiceMOP.invoiceNum FROM tbl_invoiceMOP WHERE CONCAT(tbl_invoiceMOP.invoiceNum, '-', tbl_invoiceMOP.invoiceSubNum) = CONCAT(tbl_invoice.invoiceNum, '-', tbl_invoice.invoiceSubNum))