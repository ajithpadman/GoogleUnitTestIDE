/*********************************************************************/
/*! 
* \file Mock_DemoMock.h
* 
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

#ifndef MOCK_DEMOMOCK
#define MOCK_DEMOMOCK
#include "gmock/gmock.h"
class Mock_DemoMock{
public:
 static Mock_DemoMock *mp_Instance;
 Mock_DemoMock(){mp_Instance = NULL;}
 virtual~Mock_DemoMock(){}
 inline void RegisterMock(Mock_DemoMock *mock){mp_Instance = mock;}
 inline void UnRegisterMock(){mp_Instance = NULL;}
 MOCK_METHOD0(mocked_ExtModule_function_v,void());
 MOCK_METHOD1(mocked_ExtModule_function2_v,void(u8));
 MOCK_METHOD2(mocked_ExtModule_function3_u8,u8(u16,u32));
 MOCK_METHOD3(mocked_ExtModule_function4_b,Boolean(u16 *,u32,u32 *));
};
#endif
