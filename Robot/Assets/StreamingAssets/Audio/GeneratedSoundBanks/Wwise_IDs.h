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
        static const AkUniqueID AMBIENCE_STOP = 3166394572U;
        static const AkUniqueID AMBIENT = 77978275U;
        static const AkUniqueID ARM_ATTACH = 3720067015U;
        static const AkUniqueID ARM_BREAK = 3331264981U;
        static const AkUniqueID ARM_DETATCH = 4167342225U;
        static const AkUniqueID ARM_INNODE = 4103094805U;
        static const AkUniqueID CHAINS = 640811495U;
        static const AkUniqueID CHAINS_LIGHT = 1854679430U;
        static const AkUniqueID CHIRP = 167209695U;
        static const AkUniqueID CHIRPCOMBINE = 2034861142U;
        static const AkUniqueID DIE = 445985469U;
        static const AkUniqueID DOOR_OPEN = 535830432U;
        static const AkUniqueID DOORCHIRP = 2390748151U;
        static const AkUniqueID DRONE = 2739838641U;
        static const AkUniqueID FOOTSTEPP1 = 4251213326U;
        static const AkUniqueID FOOTSTEPP2 = 4251213325U;
        static const AkUniqueID GATENOTEA = 2675281845U;
        static const AkUniqueID GATENOTEB = 2675281846U;
        static const AkUniqueID GATENOTEFINISH = 2947944241U;
        static const AkUniqueID IDLE = 1874288895U;
        static const AkUniqueID INSECTS = 3833343604U;
        static const AkUniqueID LIGHT_HITS_CRYSTAL = 3427421355U;
        static const AkUniqueID LIGHTBUZZ = 1863820488U;
        static const AkUniqueID MACHINE_CHARGE = 2356988103U;
        static const AkUniqueID MENU_ENTER = 1952436935U;
        static const AkUniqueID MENU_EXIT = 3663878895U;
        static const AkUniqueID MENU_SELECT = 4203375351U;
        static const AkUniqueID MUSIC_START = 3725903807U;
        static const AkUniqueID PICKUP_CRYSTAL = 1187533030U;
        static const AkUniqueID PLACE_CRYSTAL = 4149295597U;
        static const AkUniqueID PUSH_BOX = 2762482407U;
        static const AkUniqueID PUSH_BOX_STOP = 3321400738U;
        static const AkUniqueID SWITCH = 3202316517U;
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

        namespace CHIRPCOMBINE
        {
            static const AkUniqueID GROUP = 2034861142U;

            namespace STATE
            {
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID ON = 1651971902U;
            } // namespace STATE
        } // namespace CHIRPCOMBINE

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
                static const AkUniqueID CREDITS = 2201105581U;
                static const AkUniqueID EMPTY = 3354297748U;
                static const AkUniqueID MENU = 2607556080U;
                static const AkUniqueID P1_INTRO = 948331455U;
                static const AkUniqueID P1_INTROVIEWPOINT = 459379426U;
                static const AkUniqueID P2_FACTORYPUZZLE1 = 1415136285U;
                static const AkUniqueID P2_FACTORYPUZZLE2 = 1415136286U;
                static const AkUniqueID P2_FACTORYVIEWPOINT = 3431111895U;
                static const AkUniqueID P3_CONSTRUCTION = 166810106U;
                static const AkUniqueID P3_CONSTRUCTIONSCENERY = 2922828941U;
                static const AkUniqueID P3_CONSTRUCTIONSTORM = 3413512845U;
                static const AkUniqueID P4_CLOUDS = 1766181350U;
                static const AkUniqueID P5_CORE = 3932331850U;
                static const AkUniqueID P5_COREPUZZLE = 653832390U;
                static const AkUniqueID P6_ACHOICE = 2101370086U;
                static const AkUniqueID P6_ENDLEAVE = 3100461366U;
                static const AkUniqueID P6_ENDSACRIFICE = 2050591928U;
            } // namespace STATE
        } // namespace ENVIRONMENT

        namespace UI
        {
            static const AkUniqueID GROUP = 1551306167U;

            namespace STATE
            {
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID ON = 1651971902U;
            } // namespace STATE
        } // namespace UI

        namespace WIND
        {
            static const AkUniqueID GROUP = 1537061107U;

            namespace STATE
            {
                static const AkUniqueID OFF = 930712164U;
                static const AkUniqueID ON = 1651971902U;
            } // namespace STATE
        } // namespace WIND

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
                static const AkUniqueID GRAVEL = 2185786256U;
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
        static const AkUniqueID LIMBNUMBER = 2589579194U;
        static const AkUniqueID MASTER_VOLUME = 4179668880U;
        static const AkUniqueID STRAIN = 3538886730U;
    } // namespace GAME_PARAMETERS

    namespace TRIGGERS
    {
        static const AkUniqueID CHIRPCOMBINE = 2034861142U;
    } // namespace TRIGGERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID DRONE = 2739838641U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID SFX = 393239870U;
        static const AkUniqueID SOUNDSEED = 2786447653U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIENCE = 85412153U;
        static const AkUniqueID CHIRPS = 2407349630U;
        static const AkUniqueID CHRIP_MELODY = 4185302672U;
        static const AkUniqueID DRONE = 2739838641U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MASTER_SECONDARY_BUS = 805203703U;
        static const AkUniqueID MUSIC = 3991942870U;
        static const AkUniqueID OBJECTS = 1695690031U;
        static const AkUniqueID REVERBS = 3545700988U;
        static const AkUniqueID ROBOTS = 2494382374U;
        static const AkUniqueID SPECIAL = 3064974266U;
        static const AkUniqueID STINGERS = 2940432316U;
        static const AkUniqueID SUBMASTER = 2625406123U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID REFLECT = 243379636U;
        static const AkUniqueID REVERB_FACTORY = 2694233930U;
        static const AkUniqueID REVERB_OUTDOORS = 4227785423U;
        static const AkUniqueID REVERB_TUNNEL = 2261546958U;
        static const AkUniqueID REVERB_WAREHOUSE = 3439705185U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
