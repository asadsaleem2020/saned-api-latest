using System;
using System.Collections.Generic;
using System.Text;

namespace CoreInfrastructure.ItemInformation.ItemInformation
{
 public   class M_ExcelSheet
    {

		public string COMPANY_CODE  { get; set; }
	    public string ITEM_CODE { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_BARCODE  { get; set; }
        public string CATEGORY_ID  { get; set; }
        public string SUB_CATEGORY_ID  { get; set; }
        public string SUB_CATEGORY_DETAIL_ID  { get; set; } 
	    public string BARCODE_TYPE_ID  { get; set; }  
		public string STATUS_ID  { get; set; }  
		public string BUSSINESS_LOCATION_ID  { get; set; }  
		public decimal Alter_Qty { get; set; }
		public string PRODUCT_TYPE_ID  { get; set; } 
        public string WAREHOUSE_ID  { get; set; }  
	    public string BATCH_NO  { get; set; }  
	    public string Colour  { get; set; }  
	    public string MEASURING_UNIT_ID  { get; set; } 
	    public decimal PURCHASE_RATE { get; set; }
		public decimal SALE_RATE { get; set; }
		public decimal Packing_Qty { get; set; }
		public decimal Whole_Sale_Rate { get; set; }
		public decimal Retail_Rate { get; set; }
		public decimal MAX_LEVEL { get; set; }
		public decimal MIN_LEVEL { get; set; }
		public decimal UpperBox { get; set; }
		public decimal InnerBox { get; set; }
		public decimal PacketBox { get; set; }
		public decimal UpperPrice { get; set; }
		public decimal InnerPrice { get; set; }
		public decimal PacketPrice { get; set; }
		public string SHORT_NAME  { get; set; }  
	    public string SKU  { get; set; } 
	    public DateTime EXPIRAY_DATE { get; set; }
		public string EXPIRAY_ALERT_ID  { get; set; }  
	    public decimal EXPIRAY_ALERT { get; set; }
		public string MP_NUMBER  { get; set; } 
	    public string SP_NUMBER  { get; set; }  
	    public decimal WARRENTY_DAYS { get; set; }
		public string MODEL_NUMBER  { get; set; }  
	    public decimal WEIGHT { get; set; }
		public string DESCRIPTION  { get; set; }  
	    public string IMG_PATH  { get; set; }  
	    public decimal CURRENT_STOCK { get; set; }
		public decimal REORDER_QUANTITY { get; set; }
		public string LABEL_FORMATE  { get; set; } 
	    public DateTime PACKED_DATE { get; set; }
	    public DateTime USED_DATE { get; set; }
	    public DateTime SELL_DATE { get; set; }
		public decimal NOTSALE_QUANTITY { get; set; }
		public string CURRENT_OFFER  { get; set; }
		public decimal DISCOUNT_RATE { get; set; }
		public string OFFERDAY_ID  { get; set; }  
	    public DateTime OFFER_START_DATE { get; set; }
		public DateTime OFFER_END_DATE { get; set; }
		public string RACK_NO  { get; set; }  
	    public string ROW_NO  { get; set; }  
	    public string POSITION  { get; set; }  
	    public string FRONT_LOCATION_ID  { get; set; } 
	    public string BACK_LOCATION_ID  { get; set; } 
	    public string MULTI_BARCODE_ID  { get; set; } 
	    public DateTime CREATED_ON { get; set; }
		public string CREATED_BY  { get; set; } 
	    public bool IS_DELETED { get; set; }
		public string DELETED_BY  { get; set; } 
	    public DateTime DELETEED_ON { get; set; }
		public string UPDATE_BY  { get; set; }  
	    public DateTime UPDATE_ON { get; set; }



		 

	}
}
