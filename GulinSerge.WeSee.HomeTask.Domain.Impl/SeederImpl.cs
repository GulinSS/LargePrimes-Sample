using System;
using System.Collections.Generic;

namespace GulinSerge.WeSee.HomeTask.Domain.Impl
{
	/// <summary>
	/// Нельзя выполнить многопоточно. Создание базового решета.
	/// </summary>
	public class SeederImpl : Seeder
	{
		public IEnumerable<ulong> PrepareSeed(ulong to)
		{
			ulong rootOfMaxNumber = Convert.ToUInt64(Math.Sqrt(to));
			return Primes(rootOfMaxNumber);
		}

		private static IEnumerable<ulong> Primes(ulong number)
		{
			bool[] table = new bool[number+1];
			ulong i, j;
			// Отмечаем все числа как простые 
			for (i = 0; i < (ulong)table.LongLength; i++)
				table[i] = true;
			// Вычеркиваем лишнее 
			for (i = 2; i * i < (ulong)table.LongLength; i++)
				if (table[i])
					for (j = 2 * i; j < (ulong)table.LongLength; j += i)
						table[j] = false;
			// Выводим найденное 
			for (i = 2; i < (ulong)table.LongLength; i++)
			{
				if (table[i])
					yield return i;
			}
		}
	}
}