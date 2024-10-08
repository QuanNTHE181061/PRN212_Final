using Microsoft.EntityFrameworkCore;
using LINQ_User.Models;
class Program
{
    public static void Main(string[] args)
    {
        // Thiết lập encoding cho tiếng Việt
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        using (var context = new Prn2xxUserContext())
        {
            int personId = 1; // Ví dụ ID của người dùng
            int orderId = 1; // Ví dụ ID của đơn hàng
            int productId = 1; // Ví dụ ID của sản phẩm
            DateTime startDate = new DateTime(2023, 1, 1); // Ngày bắt đầu
            DateTime endDate = new DateTime(2023, 12, 1); // Ngày kết thúc

            // 1. Lấy tất cả đơn hàng kèm sản phẩm cho một người cụ thể
            var ordersWithProducts = context.Orders
                .Where(o => o.PersonId == personId)
                .Include(o => o.Products)
                .ToList();

            // Hiển thị thông tin đơn hàng và sản phẩm
            foreach (var order1 in ordersWithProducts)
            {
                Console.WriteLine($"Đơn hàng ID: {order1.OrderId}, Ngày đặt: {order1.OrderDate}");
                foreach (var product in order1.Products)
                {
                    Console.WriteLine($"Sản phẩm: {product.ProductName}, Giá: {product.Price}");
                }
            }

            // 2. Lấy tất cả các sản phẩm mà một người đã mua
            var purchasedProducts = context.Orders
                .Where(o => o.PersonId == personId)
                .SelectMany(o => o.Products)
                .ToList();

            // Hiển thị thông tin các sản phẩm đã mua
            Console.WriteLine("Các sản phẩm đã mua:");
            foreach (var product in purchasedProducts)
            {
                Console.WriteLine($"Sản phẩm: {product.ProductName}, Giá: {product.Price}");
            }

            // 3. Tính tổng giá trị các đơn hàng của một người
            var totalAmountSpent = context.Orders
                .Where(o => o.PersonId == personId)
                .SelectMany(o => o.Products)
                .Sum(p => p.Price);

            Console.WriteLine($"Tổng số tiền đã chi tiêu: {totalAmountSpent}");

            // 4. Lấy danh sách các đơn hàng trong một khoảng thời gian
            var ordersInTimeRange = context.Orders
                .Where(o => o.PersonId == personId && o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Include(o => o.Products)
                .ToList();

            Console.WriteLine("Đơn hàng trong khoảng thời gian:");
            foreach (var order2 in ordersInTimeRange)
            {
                Console.WriteLine($"Đơn hàng ID: {order2.OrderId}, Ngày đặt: {order2.OrderDate}");
            }

            // 5. Lấy danh sách các sản phẩm phổ biến (được mua nhiều nhất)
            var popularProducts = context.Orders
                .SelectMany(o => o.Products)
                .GroupBy(p => p.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(p => p.Count)
                .Take(3)
                .ToList();

            Console.WriteLine("Các sản phẩm phổ biến:");
            foreach (var product in popularProducts)
            {
                Console.WriteLine($"Sản phẩm ID: {product.ProductId}, Số lượng mua: {product.Count}");
            }

            // 6. Thêm một sản phẩm vào đơn hàng
            var order = context.Orders.Find(orderId);
            var productToAdd = context.Products.Find(productId);

            if (order != null && productToAdd != null)
            {
                order.Products.Add(productToAdd);
                context.SaveChanges();
                Console.WriteLine($"Đã thêm sản phẩm {productToAdd.ProductName} vào đơn hàng {orderId}");
            }

            // 7. Xóa một sản phẩm khỏi đơn hàng
            var orderWithProducts = context.Orders
                .Include(o => o.Products)
                .FirstOrDefault(o => o.OrderId == orderId);

            var productToRemove = orderWithProducts?.Products.FirstOrDefault(p => p.ProductId == productId);

            if (orderWithProducts != null && productToRemove != null)
            {
                orderWithProducts.Products.Remove(productToRemove);
                context.SaveChanges();
                Console.WriteLine($"Đã xóa sản phẩm {productToRemove.ProductName} khỏi đơn hàng {orderId}");
            }

            // 8. Cập nhật thông tin sản phẩm (ví dụ: tên, giá)
            var productToUpdate = context.Products.Find(productId);

            if (productToUpdate != null)
            {
                productToUpdate.ProductName = "Tên sản phẩm mới";
                productToUpdate.Price = 39.99m;
                context.SaveChanges();
                Console.WriteLine($"Đã cập nhật thông tin sản phẩm {productToUpdate.ProductName}");
            }

            // 9. Lấy tất cả các sở thích của một người cụ thể
            var hobbies = context.Persons
                .Where(p => p.PersonId == personId)
                .SelectMany(p => p.Hobbies)
                .ToList();

            Console.WriteLine("Các sở thích của người dùng:");
            foreach (var hobby in hobbies)
            {
                Console.WriteLine($"Sở thích: {hobby.HobbyName}");
            }

            // 10. Đếm số lượng đơn hàng của một người
            var orderCount = context.Orders
                .Where(o => o.PersonId == personId)
                .Count();

            Console.WriteLine($"Số lượng đơn hàng của người dùng: {orderCount}");

            // 11. Lấy thông tin người dùng và các sản phẩm đã mua kèm tổng chi tiêu
            var userPurchases = context.Persons
                .Where(p => p.PersonId == personId)
                .Select(p => new
                {
                    FullName = p.FirstName + " " + p.LastName,
                    TotalSpent = p.Orders.SelectMany(o => o.Products).Sum(pr => pr.Price),
                    ProductsBought = p.Orders.SelectMany(o => o.Products).ToList()
                })
                .FirstOrDefault();

            Console.WriteLine($"Người dùng {userPurchases.FullName} đã mua {userPurchases.ProductsBought.Count} sản phẩm với tổng chi tiêu là {userPurchases.TotalSpent}.");
            Console.ReadLine();
        }
    }
}