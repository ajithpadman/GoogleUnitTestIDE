/*********************************************************************/
/*! 
* \file Mock_DemoMock.cpp
* 
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

#include "Mock_DemoMock.h"
Mock_DemoMock* Mock_DemoMock::mp_Instance = 0;
/*********************************************************************/
/*! 
*  \class Mock_DemoMock
* Mock class for the module DemoMock
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

/*********************************************************************/
/*! \fn ExtModule_function_v
* \brief Mock for the function ExtModule_function_v()
*/
/*********************************************************************/
void ExtModule_function_v()
{
  if(NULL != Mock_DemoMock::mp_Instance)
  {
     Mock_DemoMock::mp_Instance->mocked_ExtModule_function_v();
  }
  else
  {
    Mock_DemoMock l_MockObject;
      l_MockObject.mocked_ExtModule_function_v();
  }
}
/*********************************************************************/
/*! \fn ExtModule_function2_v
* \brief Mock for the function ExtModule_function2_v()
*/
/*********************************************************************/
void ExtModule_function2_v(u8 l_arg0){
  if(NULL != Mock_DemoMock::mp_Instance)
  {
     Mock_DemoMock::mp_Instance->mocked_ExtModule_function2_v( l_arg0);
  }
  else
  {
    Mock_DemoMock l_MockObject;
      l_MockObject.mocked_ExtModule_function2_v( l_arg0);
  }
}
/*********************************************************************/
/*! \fn ExtModule_function3_u8
* \brief Mock for the function ExtModule_function3_u8()
*/
/*********************************************************************/
u8 ExtModule_function3_u8(u16 l_arg0,u32 l_arg1){
  if(NULL != Mock_DemoMock::mp_Instance)
  {
    return Mock_DemoMock::mp_Instance->mocked_ExtModule_function3_u8( l_arg0, l_arg1);
  }
  else
  {
    Mock_DemoMock l_MockObject;
static unsigned char Mock_DemoMockRetVal_ActualType =  std::numeric_limits < unsigned char >::min();

static u8 Mock_DemoMockRetVal = Mock_DemoMockRetVal_ActualType;


EXPECT_CALL
(
l_MockObject,
mocked_ExtModule_function3_u8( l_arg0, l_arg1)
)
.Times(::testing::AnyNumber())
.WillRepeatedly(::testing::Return(Mock_DemoMockRetVal));
    return l_MockObject.mocked_ExtModule_function3_u8( l_arg0, l_arg1);
  }
}
/*********************************************************************/
/*! \fn ExtModule_function4_b
* \brief Mock for the function ExtModule_function4_b()
*/
/*********************************************************************/
Boolean ExtModule_function4_b(u16 * l_arg0,u32 l_arg1,u32 * l_arg2){
  if(NULL != Mock_DemoMock::mp_Instance)
  {
    return Mock_DemoMock::mp_Instance->mocked_ExtModule_function4_b( l_arg0, l_arg1, l_arg2);
  }
  else
  {
    Mock_DemoMock l_MockObject;
static unsigned char Mock_DemoMockRetVal_ActualType =  std::numeric_limits < unsigned char >::min();

static Boolean Mock_DemoMockRetVal = Mock_DemoMockRetVal_ActualType;


EXPECT_CALL
(
l_MockObject,
mocked_ExtModule_function4_b( l_arg0, l_arg1, l_arg2)
)
.Times(::testing::AnyNumber())
.WillRepeatedly(::testing::Return(Mock_DemoMockRetVal));
    return l_MockObject.mocked_ExtModule_function4_b( l_arg0, l_arg1, l_arg2);
  }
}
