using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Reflection;
using System.Diagnostics;

namespace Common.Helpers
{
	public class DataTableListConversions
	{
		public DataTable ListToDataTable<T>(List<T> items)
			where T : class
		{
			return ListToDataTable<T>(items, new DataTableEx());
		}

		public DataTable ListToDataTable<T>(List<T> items, DataTable table)
			where T : class
		{
			PropertyDescriptorCollection Properties = TypeDescriptor.GetProperties(typeof(T));

			for (int Index = 0; Index < Properties.Count; Index++)
			{
				PropertyDescriptor Property = Properties[Index];

				if (true == Property.PropertyType.IsGenericType && typeof(Nullable<>) == Property.PropertyType.GetGenericTypeDefinition())
					table.Columns.Add(Property.Name, Property.PropertyType.GetGenericArguments()[0]);
				else
					table.Columns.Add(Property.Name, Property.PropertyType);
			}

			object[] Values = new object[Properties.Count];

			foreach (T Item in items)
			{
				for (int Index = 0; Index < Values.Length; Index++)
					Values[Index] = Properties[Index].GetValue(Item);

				table.Rows.Add(Values);
			}

			return table;
		}

		public List<T> DataTableToList<T>(DataTable table)
			where T : class, new()
		{
			List<T> Items = new List<T>();

			foreach (DataRow Row in table.Rows)
			{
				T Obj = new T();

				foreach (PropertyInfo Prop in Obj.GetType().GetProperties())
				{
					try
					{
						PropertyInfo PropInfo = Obj.GetType().GetProperty(Prop.Name);
						PropInfo.SetValue(Obj, Convert.ChangeType(Row[Prop.Name], PropInfo.PropertyType), null);
					}
					catch (Exception ex)
					{
						throw new ArgumentException(string.Format("Conversion failed on {0}...", Prop.Name), ex);
					}
				}

				Items.Add(Obj);
			}

			return Items;
		}
	}
}
