using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CodeGeneration
{
   class Program
   {
      public static int GetRandomInt( RNGCryptoServiceProvider rnd, int max )
      {
         var randomBytes = new byte[4];
         int maxRandomValue = GetMaxDividend( Int32.MaxValue, max );
         int value;
         do
         {
            rnd.GetBytes( randomBytes );
            value = BitConverter.ToInt32( randomBytes, 0 ) & Int32.MaxValue;
         } while ( value >= maxRandomValue );
         return value % max;
      }

      public static string RandomString( int length )
      {
         const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
         var resultString = new StringBuilder();
         using ( RNGCryptoServiceProvider rnd = new RNGCryptoServiceProvider() )
         {
            while ( length-- > 0 )
            {
               resultString.Append( valid[GetRandomInt( rnd, valid.Length )] );
            }
         }
         return resultString.ToString();
      }

      public static int GetMaxDividend( int dividend, int divisor )
      {
         var remainder = dividend % divisor;
         var result = dividend - remainder;

         return result;
      }

      static void Main( string[] args )
      {

         var resultsDict = new Dictionary<string, int>();

         for ( int i = 0; i < 100000; i++ )
         {
            var code = RandomString( 6 );
            if ( resultsDict.ContainsKey( code ) ) resultsDict[code]++;
            else
            {
               resultsDict.Add( code, 1 );
            }
         }
         using ( StreamWriter sw = new StreamWriter( "generatedCodes.txt" ) )
         {
            foreach ( var resultsDictKey in resultsDict.Keys )
            {
               sw.WriteLine( $"{resultsDictKey}   {resultsDict[resultsDictKey]}" );
            }
         }
      }
   }
}
