#include "stdio.h"
typedef enum
{
	ONE,
	TWO
}enumType;
enumType VariableEnum;
int x[10];
float* testPointer;


typedef struct  element
{
	int x;
	float y;
	struct  element *p_str;
	
}MyStruct;
typedef int MYINt;
MYINt gloablVariable = 0;
struct  element variableStruct;

struct structure
{
 int x;
 float y;
 MYINt z;
};
 enumType function(int x, float y)
 {
   int local= 0;
   function2();
 }
 MyStruct* function2()
 {
	 int x;
	 float sss;
	 MyStruct * retVal = null;
	function(1,1);
	 function2();
   return retVal;
 }
 void functionwitharray(const int array[],float floatArr[3])
 {
      int x;
	 float sss;
	 MyStruct * retVal = null;
	  function(1,1);
	 function2();
 }