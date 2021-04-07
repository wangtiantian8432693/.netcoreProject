

using AutoMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
//using AutoMapper;

namespace Common
{
    /// <summary>
    ///  AutoMapper扩展方法
    /// </summary>
    public static class AutoMapperExtension
    {
        public static List<T1> MapTo<T, T1>(this object source) //this IEnumerable source
        {
            source.ToString();
            var config = new MapperConfiguration(cfg => cfg.CreateMap<T, T1>());
            IMapper mapper = config.CreateMapper();
            return mapper.Map<List<T1>>(source);
        }
    }
}
