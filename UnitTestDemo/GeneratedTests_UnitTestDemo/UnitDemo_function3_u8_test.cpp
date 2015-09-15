/*********************************************************************/
/*! 
* \file 
* 
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

/*Header Files*/
#include "gtest/gtest.h"
#include "gmock/gmock.h"
#include "tr1/tuple"
#include "limits"
/*Namespace References*/
using ::testing::TestWithParam;
using ::testing::Values;
using ::testing::Combine;
using ::testing::Bool;
using ::testing::Range;
using ::testing::ExitedWithCode;

extern u8 UnitDemo_function3_u8(u16,u32);
/*********************************************************************/
/*! 
*  \class UnitDemo_function3_u8_BoundaryFixture
* Boundary test Suit for method UnitDemo_function3_u8
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

class UnitDemo_function3_u8_BoundaryFixture:public TestWithParam<std::tr1::tuple<u16,u32> >
{
public:
/*********************************************************************/
/*! 
* \fn void SetUp()
* Set up for each tests 
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

virtual void SetUp()
{
}
/*********************************************************************/
/*! 
* \fn void TearDown()
* Tear Down each tests 
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

virtual void TearDown()
{
}
};
/*********************************************************************/
/*! 
* \fn u16 getParamMax_0()
* Get the maximum value for Parameter 0
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

static u16 getParamMax_0()
{
static unsigned short member_0_ActualType =  std::numeric_limits < unsigned short >::max();

static u16 member_0 = member_0_ActualType;


return member_0;
}
/*********************************************************************/
/*! 
* \fn u16 getParamMin_0()
* Get the minimum value for Parameter 0
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

static u16 getParamMin_0()
{
static unsigned short member_0_ActualType =  std::numeric_limits < unsigned short >::min();

static u16 member_0 = member_0_ActualType;


return member_0;
}
/*********************************************************************/
/*! 
* \fn u32 getParamMax_1()
* Get the maximum value for Parameter 1
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

static u32 getParamMax_1()
{
static unsigned int member_1_ActualType =  std::numeric_limits < unsigned int >::max();

static u32 member_1 = member_1_ActualType;


return member_1;
}
/*********************************************************************/
/*! 
* \fn u32 getParamMin_1()
* Get the minimum value for Parameter 1
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

static u32 getParamMin_1()
{
static unsigned int member_1_ActualType =  std::numeric_limits < unsigned int >::min();

static u32 member_1 = member_1_ActualType;


return member_1;
}
/*********************************************************************/
/*! 
* \fn INSTANTIATE_TEST_CASE_P
* Instantiate the Test value Fixture 
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

INSTANTIATE_TEST_CASE_P
(
UnitDemo_function3_u8_BoundaryTest_,
UnitDemo_function3_u8_BoundaryFixture,
Combine
(
Values(getParamMin_0(),getParamMax_0()),Values(getParamMin_1(),getParamMax_1())
)
);
/*********************************************************************/
/*! 
* \fn TEST
* Test case for Boundary testing Method UnitDemo_function3_u8
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

TEST_P(UnitDemo_function3_u8_BoundaryFixture,UnitDemo_function3_u8_Test)
{
EXPECT_EXIT
(
UnitDemo_function3_u8(
std::tr1::get < 0 > (GetParam()),std::tr1::get < 1 > (GetParam())
);exit(0),ExitedWithCode(0),""
);
}



