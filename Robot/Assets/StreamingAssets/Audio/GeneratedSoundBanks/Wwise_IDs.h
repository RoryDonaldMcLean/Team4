/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID ACTIVATE_CRYSTAL = 3781481881U;
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID ARM_ATTACH = 3720067015U;
        static const AkUniqueID ARM_BREAK = 3331264981U;
        static const AkUniqueID ARM_DETATCH = 4167342225U;
        static const AkUniqueID CHIRP = 167209695U;
        static const AkUniqueID DRONE = 2739838641U;
        static const AkUniqueID FOOTSTEP = 1866025847U;
        static const AkUniqueID INSECTS = 3833343604U;
        static const AkUniqueID LIGHT_HITS_CRYSTAL = 3427421355U;
        static const AkUniqueID LIGHTBUZZ = 1863820488U;
        static const AkUniqueID MUSIC_START = 3725903807U;
        static const AkUniqueID PICKUP_CRYSTAL = 1187533030U;
        static const AkUniqueID PLACE_CRYSTAL = 4149295597U;
        static const AkUniqueID TARPFLAP = 1728682787U;
        static const AkUniqueID THECORE = 1875437039U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace CHIRP_TYPE
        {
            static const AkUniqueID GROUP = 1441557762U;

            namespace STATE
            {
                static const AkUniqueID HAPPY = 1427264549U;
                static const AkUniqueID HERE = 3196610211U;
                static const AkUniqueID SAD = 443572635U;
                static const AkUniqueID THERE = 1067353181U;
            } // namespace STATE
        } // namespace CHIRP_TYPE

        namespace DRONE_MODULATOR
        {
            static const AkUniqueID GROUP = 2978482827U;

            namespace STATE
            {
                static const AkUniqueID COLOUR_CHANGE = 2537050904U;
                static const AkUniqueID COMBINER = 1720710682U;
                static const AkUniqueID COMPLETE = 2631133988U;
                static const AkUniqueID HIT_SWITCH = 190644483U;
                static const AkUniqueID HIT_WALL = 4103990255U;
                static const AkUniqueID REFLECTOR = 2858403387U;
                static const AkUniqueID SPLITTER = 1318750168U;
                static const AkUniqueID START = 1281810935U;
                static const AkUniqueID THROUGH_BARRIER = 3307507876U;
            } // namespace STATE
        } // namespace DRONE_MODULATOR

        namespace ENVIRONMENT
        {
            static const AkUniqueID GROUP = 1229948536U;

            namespace STATE
            {
                static const AkUniqueID E1P1 = 504457814U;
                static const AkUniqueID E1T1 = 437347370U;
                static const AkUniqueID E2T2 = 134805810U;
                static const AkUniqueID E3P2 = 302832231U;
                static const AkUniqueID E3P3 = 302832230U;
                static const AkUniqueID E4T3 = 470063997U;
                static const AkUniqueID E5P4 = 101103479U;
                static const AkUniqueID E6P5 = 335548861U;
                static const AkUniqueID E6T4 = 402659296U;
                static const AkUniqueID E7T5 = 503472104U;
                static const AkUniqueID E7T6 = 503472107U;
                static const AkUniqueID MENU = 2607556080U;
            } // namespace STATE
        } // namespace ENVIRONMENT

    } // namespace STATES

    namespace SWITCHES
    {
        namespace FOOTSTEPS
        {
            static const AkUniqueID GROUP = 2385628198U;

            namespace SWITCH
            {
                static const AkUniqueID CONCRETE = 841620460U;
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRASS = 4248645337U;
                static const AkUniqueID METAL = 2473969246U;
                static const AkUniqueID SAND = 803837735U;
                static const AkUniqueID TILE = 2637588553U;
            } // namespace SWITCH
        } // namespace FOOTSTEPS

        namespace LIMBNUMBER
        {
            static const AkUniqueID GROUP = 2589579194U;

            namespace SWITCH
            {
                static const AkUniqueID LIMB1 = 2372674558U;
                static const AkUniqueID LIMB2 = 2372674557U;
                static const AkUniqueID LIMB3 = 2372674556U;
                static const AkUniqueID LIMB4 = 2372674555U;
            } // namespace SWITCH
        } // namespace LIMBNUMBER

        namespace PLAYERID
        {
            static const AkUniqueID GROUP = 3674684721U;

            namespace SWITCH
            {
                static const AkUniqueID PLAYER_1 = 2768693346U;
                static const AkUniqueID PLAYER_2 = 2768693345U;
            } // namespace SWITCH
        } // namespace PLAYERID

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID CHIRPS_COMBINE = 4148654648U;
        static const AkUniqueID ENVIRONMENTEFFECTS = 2237703306U;
        static const AkUniqueID WIND_SPEED = 3110594711U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENCE = 85412153U;
        static const AkUniqueID CHIRPS = 2407349630U;
        static const AkUniqueID DRONE = 2739838641U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTER_SECONDARY_BUS = 805203703U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID OBJECTS = 1695690031U;
        static const AkUniqueID ROBOTS = 2494382374U;
    } // namespace BUSSES

}// namespace AK

#endif // __WWISE_IDS_H__
