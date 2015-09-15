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

extern void UnitDemo_function2_v(u8);
/*********************************************************************/
/*! 
*  \class UnitDemo_function2_v_BoundaryFixture
* Boundary test Suit for method UnitDemo_function2_v
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

class UnitDemo_function2_v_BoundaryFixture:public TestWithParam<u8 >
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
* \fn u8 getParamMax_0()
* Get the maximum value for Parameter 0
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

static u8 getParamMax_0()
{
static unsigned char member_0_ActualType =  std::numeric_limits < unsigned char >::max();

static u8 member_0 = member_0_ActualType;


return member_0;
}
/*********************************************************************/
/*! 
* \fn u8 getParamMin_0()
* Get the minimum value for Parameter 0
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

static u8 getParamMin_0()
{
static unsigned char member_0_ActualType =  std::numeric_limits < unsigned char >::min();

static u8 member_0 = member_0_ActualType;


return member_0;
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
UnitDemo_function2_v_BoundaryTest_,
UnitDemo_function2_v_BoundaryFixture,
Values(getParamMin_0(),getParamMax_0())
);
/*********************************************************************/
/*! 
* \fn TEST
* Test case for Boundary testing Method UnitDemo_function2_v
* \author MMEMEA\F11786B
* \version 1.0
* \date 9/15/2015 12:00:00 AM
*/
/*********************************************************************/

TEST_P(UnitDemo_function2_v_BoundaryFixture,UnitDemo_function2_v_Test)
{
EXPECT_EXIT
(
UnitDemo_function2_v(
GetParam()
);exit(0),ExitedWithCode(0),""
);
}



