using System.Collections.ObjectModel;

namespace Rocky_Utility;

public class WC
{
    public static string ImagePath = "/Images/Productimages";
    public static string SessionCart = "ShoppingCartSession";
    public static string SessionInquiryId = "InquirySession";
    public const string AdminRole = "Admin";
    public const string CustomerRole = "Customer";

    public static string AdminEmail = "valentine.onodu.247160@unn.edu.ng";
    public static string AdminPassword = "somtochukwu";
    public static string CustomerEmail = "da.valchi@gmail.com";


    public static string Success = "Success";
    public static string Error = "Error";
    public static string StatusPending = "Pending";
    public static string StatusApproved = "Approved";
    public static string StatusShipped = "Shipped";
    public static string StatusInProgress = "Progress";
    public static string StatusCancel = "Cancel";
    public static string StatusRefunded = "Refunded";

    public static string FaceBookId = "1486844818405809";
    public static string FaceBookSecret = "52629ca907e0ff7de7937789aab50eef";

    public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>
        (
            new List<string>
            {
                StatusApproved,StatusCancel,StatusPending,StatusPending,StatusRefunded,StatusShipped
            }
        );
}