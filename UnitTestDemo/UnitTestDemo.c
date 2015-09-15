#include "UnitTestDemo.h"
#include "ExtModule.h"
void UnitDemo_function_v(void)
{
   printf("I am in Function UnitDemo_function_v\n");
   ExtModule_function_v();
}
void UnitDemo_function2_v(u8 l_u8value)
{
   printf("I am in Function UnitDemo_function2_v\n");
   ExtModule_function2_v(l_u8value);
}
u8 UnitDemo_function3_u8(u16 l_u16value,u32 l_u32Value)
{
   printf("I am in Function UnitDemo_function3_u8\n");
   return ExtModule_function3_u8(l_u16value,l_u32Value);
}
Boolean UnitDemo_function4_b(u16 *l_pu16Value,u32 l_u32Value,u32 * l_pu32Value)
{
   printf("I am in Function UnitDemo_function4_b\n");
   if(ExtModule_function4_b(l_pu16Value,l_u32Value,l_pu32Value) == TRUE)
   {
         UnitDemo_function_v();
         return TRUE;
   }
   else
   {
     return FALSE;
   }
}