using System;
using System.Collections.Generic;
using System.Globalization;

namespace OOP
{
    class Taikhoan
    {
        private string _soTK, _chuTK;
        private decimal _soDu;

        public string SoTK { get => _soTK; private set => _soTK = value; }

        public string ChuTK
        {
            get => _chuTK;
            set => _chuTK = string.IsNullOrWhiteSpace(value) ? "N/A" : value.Trim();
        }

        public decimal SoDu
        {
            get => _soDu;
            private set => _soDu = value < 0 ? 0 : value;
        }

        public Taikhoan(string soTK, string chuTK, decimal soDuBanDau = 0m)
        {
            SoTK = soTK;
            ChuTK = chuTK;
            SoDu = soDuBanDau;
        }

        public void Nap(decimal amt)
        {
            if (amt <= 0) throw new ArgumentException("Số tiền nạp phải > 0");
            SoDu += amt;
        }

        public void Rut(decimal amt)
        {
            if (amt <= 0) throw new ArgumentException("Số tiền rút phải > 0");
            if (amt > SoDu) throw new InvalidOperationException("Số dư không đủ");
            SoDu -= amt;
        }

        public void Chuyen(Taikhoan to, decimal amt)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (ReferenceEquals(this, to)) throw new ArgumentException("Không thể chuyển cho chính mình");
            Rut(amt);
            to.Nap(amt);
        }

        public override string ToString() => $"{SoTK,-5} | {ChuTK,-20} | {SoDu,12:C0}";
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            var vi = CultureInfo.GetCultureInfo("vi-VN");
            // Nếu muốn dùng culture trong format mặc định:
            // System.Threading.Thread.CurrentThread.CurrentCulture = vi;

            var ds = new List<Taikhoan>
            {
                new Taikhoan(soTK: "001", chuTK: " Nguyễn A ", soDuBanDau: 2_000_000m),
                new Taikhoan(soTK: "002", chuTK: " Trần B ", soDuBanDau: 500_000m),
                new Taikhoan(soTK: "003", chuTK: " Lê C ", soDuBanDau: 0m),
            };

            while (true)
            {
                Console.WriteLine("\n=== TÀI KHOẢN NGÂN HÀNG ===");
                Console.WriteLine("1) Danh sách  2) Nạp  3) Rút  4) Chuyển  5) Thoát  7) Nghỉ ");
                Console.Write("Chọn: ");
                string c = Console.ReadLine() ?? "";

                try
                {
                    if (c == "1")
                    {
                        Console.WriteLine("SoTK | Chủ TK              |     Số dư");
                        Console.WriteLine("-------------------------------------------");
                        ds.ForEach(tk => Console.WriteLine(tk));
                    }
                    else if (c == "2")
                    {
                        Console.Write("Số TK: "); string s = Console.ReadLine() ?? "";
                        Console.Write("Số tiền nạp: "); decimal x = decimal.Parse(Console.ReadLine() ?? "0");
                        var acc = ds.Find(t => t.SoTK == s);
                        if (acc == null) throw new ArgumentException("Số TK không tồn tại");
                        acc.Nap(x);
                        Console.WriteLine("→ Nạp thành công.");
                    }
                    else if (c == "3")
                    {
                        Console.Write("Số TK: "); string s = Console.ReadLine() ?? "";
                        Console.Write("Số tiền rút: "); decimal x = decimal.Parse(Console.ReadLine() ?? "0");
                        var acc = ds.Find(t => t.SoTK == s);
                        if (acc == null) throw new ArgumentException("Số TK không tồn tại");
                        acc.Rut(x);
                        Console.WriteLine("→ Rút thành công.");
                    }
                    else if (c == "4")
                    {
                        Console.Write("Từ (SoTK): "); string s1 = Console.ReadLine() ?? "";
                        Console.Write("Đến (SoTK): "); string s2 = Console.ReadLine() ?? "";
                        Console.Write("Số tiền: "); decimal x = decimal.Parse(Console.ReadLine() ?? "0");
                        var from = ds.Find(t => t.SoTK == s1);
                        var to = ds.Find(t => t.SoTK == s2);
                        if (from == null || to == null) throw new ArgumentException("Số TK không tồn tại");
                        from.Chuyen(to, x);
                        Console.WriteLine("→ Chuyển thành công.");
                    }
                    else if (c == "5")
                    {
                        return;
                    }
                    else Console.WriteLine("Lựa chọn không hợp lệ.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Lỗi: " + ex.Message);
                }
            }
        }
    }
}
