export const TokenHelper = {
    getQueryName : function(queryType:number)
    {
      switch (queryType) {
        case 1:
          return "General"
          break;
        case 2:
          return "New product"
          break;
        case 3:
          return "Others"
          break;
      }
      return ""
    },
    getStatusName: function(statusType:number)
    {
      switch (statusType) {
        case 1:
          return "Pending"
          break;
        case 2:
          return "Processing"
          break;
        case 3:
          return "Resolved"
          break;
      }
      return ""
    },
    getAllQueryName : function() {
      return [{
          key: 1
          , value: this.getQueryName(1)
      }
          , {
          key: 2
          , value: this.getQueryName(2)
      }
          , {
          key: 3
          , value: this.getQueryName(3)
      }
          ,]
    }
  };

  