using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BKZalo.Core.Entities
{
    // thông tin bắt buộc nhập
    [AttributeUsage(AttributeTargets.Property)]
    public class Requied : Attribute
    {

    }

    // thông tin không được phép trùng
    [AttributeUsage(AttributeTargets.Property)]
    public class NotAllowDuplicate : Attribute
    {

    }

    // Khóa chính
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKey : Attribute
    {

    }

    // có trong model nhưng không có trong bảng db
    [AttributeUsage(AttributeTargets.Property)]
    public class NotMap : Attribute
    {

    }
}
