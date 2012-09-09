using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace GulinSerge.WeSee.HomeTask.Domain.Tests
{
	[TestFixture]
	public class Class1
	{
		[Test]
		public void Test1()
		{
			ulong maxNumber = 50;
			ulong minNumber = 2;
			ulong rootOfMaxNumber = Convert.ToUInt64(Math.Sqrt(maxNumber));

			var primes = Primes(rootOfMaxNumber).ToArray();

			ulong BlockSize = 10;
			for (ulong I = minNumber; I < maxNumber; I += BlockSize)
			{
				bool[] sieve = new bool[BlockSize];
				for (ulong i = 0; i < (ulong)sieve.LongLength; i++)
					sieve[i] = true;

				for (ulong i = 0; i < (uint)primes.LongLength; i++)
				{
					bool first = true;

					ulong h = I % primes[i];
					ulong j = h == 0 ? 0 : primes[i] - h;
					for (; j < BlockSize; j += primes[i])
					{
						if (first && primes.First() <= i+I && primes.Last() >= i+I)
						{
							first = false;
							continue;
						}
						sieve[j] = false;
					}
				}

				for (ulong i = 0; i < (ulong)sieve.LongLength; i++)
				{
					if (sieve[i])
						Debug.WriteLine(i + I);
				}
			}
		}

		// Define other methods and classes here
		IEnumerable<ulong> Primes(ulong number)
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
