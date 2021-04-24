# TeamsEXILED

<a href="https://github.com/boogaeye/AdvancedTeamCreation/releases"><img src="https://img.shields.io/github/v/release/boogaeye/AdvancedTeamCreation?include_prereleases&label=Release" alt="Releases"></a>
<a href="https://github.com/boogaeye/AdvancedTeamCreation/releases"><img src="https://img.shields.io/github/downloads/boogaeye/AdvancedTeamCreation/total?label=Downloads" alt="Downloads"></a>

- **Visit the wiki to get the best description:**
https://github.com/boogaeye/AdvancedTeamCreation/wiki

Default Config:

```yml

advanced_team_creation:
  is_enabled: true
  # if this is true then it will allow one player to be alive in the round when someone kills themselves(This is basically to give them another chance before ending the game)
  allow1_player: false
  team_kill_broadcast: You got teamkilled report this to the admins if you dont think its an accident
  killed_by_nonfriendly_player: You didnt get team killed you where probably killed by someone who looks like you but isnt
  # allows friendly teams to hurt eachother no matter what hurts them
  friendly_fire: false
  # If enabled the RoleHint message will be displayed as a hint, else as a broadcast
  use_hints: true
  # Should display the description of the customitem when given?
  display_description: false
  debug: false
  # All team names have to be lowercase otherwise IT WILL NOT LET YOU SPAWN CORRECTLY!
  teams:
  -
  # sets if the teams active
    active: true
    # Sets the team name MUST be lowercase
    name: goc
    # Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)
    subclasses:
    -
    # Sets the Subclasses name
      name: rookie
      # sets the subclasses HP
      h_p: 135
      # sets what this class has
      inventory:
      - KeycardNTFLieutenant
      - Radio
      - Medkit
      - 5
      - 2
      - 2
      - 0
      # Define ammo so that this class has this ammo
      ammo:
        Nato762: 200
      # Sets what this role is supposed to look like
      model_role: ChaosInsurgency
      # sets the role name on the player to easily define who you are
      role_name: <color=yellow>GOC</color>
      # What message is displayed for the role itself
      role_message: You are the GOC
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: -1
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - goc
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - mtf
    - chi
    - gru
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - cdp
    - tta
    - rsc
    - opcf
    - aes
    # Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations
    spawn_types:
    - ChaosInsurgency
    # defines who wins at the end of the round if the Requirements perameter is accepted
    team_leaders: FacilityForces
    # Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message
    cassie_message_m_t_f_spawn: 
    # Makes a Chaos cassie message when the team spawns
    cassie_message_chaos_message: pitch_0.6 .g5 .g5 .g5 pitch_1 the g o c has entered the facility bell_end
    # set this to 0 to prevent the cassie announcement for chaos
    cassie_message_chaos_announce_chance: 100
    # Sets where this team spawns
    spawn_location: Normal
    # makes it where if this team is the latest spawn it will spawn the assigned escapees to this team if they are defined in this config
    escape_change:
    - Scientist
    - DClass
    color: yellow
    # the chance this team will spawn if its been selected
    chance: 65
  -
  # sets if the teams active
    active: true
    # Sets the team name MUST be lowercase
    name: gru
    # Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)
    subclasses:
    -
    # Sets the Subclasses name
      name: rookie
      # sets the subclasses HP
      h_p: 150
      # sets what this class has
      inventory:
      - GunE11SR
      - KeycardChaosInsurgency
      - Adrenaline
      - WeaponManagerTablet
      - GrenadeFlash
      - 7
      - 6
      # Define ammo so that this class has this ammo
      ammo:
        Nato762: 200
      # Sets what this role is supposed to look like
      model_role: FacilityGuard
      # sets the role name on the player to easily define who you are
      role_name: <color=yellow>GRU</color>
      # What message is displayed for the role itself
      role_message: ''
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: -1
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - gru
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - mtf
    - chi
    - goc
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - tta
    - cdp
    - rsc
    - aes
    # Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations
    spawn_types:
    - NineTailedFox
    # defines who wins at the end of the round if the Requirements perameter is accepted
    team_leaders: FacilityForces
    # Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message
    cassie_message_m_t_f_spawn: .g5 .g5 the g r u has entered the facility. there are {SCP} scpsubjects
    # Makes a Chaos cassie message when the team spawns
    cassie_message_chaos_message: 
    # set this to 0 to prevent the cassie announcement for chaos
    cassie_message_chaos_announce_chance: 100
    # Sets where this team spawns
    spawn_location: Normal
    # makes it where if this team is the latest spawn it will spawn the assigned escapees to this team if they are defined in this config
    escape_change:
    - Scientist
    - DClass
    color: yellow
    # the chance this team will spawn if its been selected
    chance: 65
  -
  # sets if the teams active
    active: true
    # Sets the team name MUST be lowercase
    name: tta
    # Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)
    subclasses:
    -
    # Sets the Subclasses name
      name: officer
      # sets the subclasses HP
      h_p: 100
      # sets what this class has
      inventory:
      - GunUSP
      - KeycardFacilityManager
      - Adrenaline
      - WeaponManagerTablet
      - 0
      - 0
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 200
      # Sets what this role is supposed to look like
      model_role: NtfScientist
      # sets the role name on the player to easily define who you are
      role_name: <color=red>TTA</color>
      # What message is displayed for the role itself
      role_message: >-
        You are the TTA

        Kill everything in sight
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: -1
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - tta
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral: []
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - chi
    - scp
    - goc
    - gru
    - mtf
    - cdp
    - rsc
    - opcf
    - aes
    # Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations
    spawn_types:
    - NineTailedFox
    - ChaosInsurgency
    # defines who wins at the end of the round if the Requirements perameter is accepted
    team_leaders: Anomalies
    # Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message
    cassie_message_m_t_f_spawn: pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C has entered the facility bell_end
    # Makes a Chaos cassie message when the team spawns
    cassie_message_chaos_message: pitch_0.1 .g3 .g3 .g3 pitch_1 The Tactical Target Agent C unit {nato} {SCP} has entered the facility bell_end
    # set this to 0 to prevent the cassie announcement for chaos
    cassie_message_chaos_announce_chance: 75
    # Sets where this team spawns
    spawn_location: SurfaceNuke
    # makes it where if this team is the latest spawn it will spawn the assigned escapees to this team if they are defined in this config
    escape_change:
    - Scientist
    - DClass
    color: red
    # the chance this team will spawn if its been selected
    chance: 50
  -
  # sets if the teams active
    active: true
    # Sets the team name MUST be lowercase
    name: opcf
    # Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)
    subclasses:
    -
    # Sets the Subclasses name
      name: commander
      # sets the subclasses HP
      h_p: 160
      # sets what this class has
      inventory:
      - GunLogicer
      - GunProject90
      - KeycardChaosInsurgency
      - Radio
      - Medkit
      - Medkit
      - 0
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
      # Sets what this role is supposed to look like
      model_role: FacilityGuard
      # sets the role name on the player to easily define who you are
      role_name: OPCF Commander
      # What message is displayed for the role itself
      role_message: >-
        You are part of the branch of the NTF

        <color=blue>Operation Chaos Force</color>

        Kill everything you see and help the MTF
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: 1
    -
    # Sets the Subclasses name
      name: officer
      # sets the subclasses HP
      h_p: 120
      # sets what this class has
      inventory:
      - GunLogicer
      - GunProject90
      - KeycardNTFLieutenant
      - Radio
      - Medkit
      - Painkillers
      - 0
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
      # Sets what this role is supposed to look like
      model_role: NtfLieutenant
      # sets the role name on the player to easily define who you are
      role_name: OPCF Officer
      # What message is displayed for the role itself
      role_message: >-
        You are part of the branch of the NTF

        <color=blue>Operation Chaos Force</color>

        Kill everything you see and help the MTF
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: 2
    -
    # Sets the Subclasses name
      name: cadet
      # sets the subclasses HP
      h_p: 100
      # sets what this class has
      inventory:
      - GunProject90
      - KeycardGuard
      - Radio
      - Painkillers
      - GrenadeFlash
      - 0
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
      # Sets what this role is supposed to look like
      model_role: NtfCadet
      # sets the role name on the player to easily define who you are
      role_name: OPCF Cadet
      # What message is displayed for the role itself
      role_message: >-
        You are part of the branch of the NTF

        <color=blue>Operation Chaos Force</color>

        Kill everything you see and help the MTF
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: -1
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - opcf
    - mtf
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - rsc
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - chi
    - scp
    - goc
    - gru
    - cdp
    - tta
    - aes
    # Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations
    spawn_types:
    - ChaosInsurgency
    # defines who wins at the end of the round if the Requirements perameter is accepted
    team_leaders: FacilityForces
    # Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message
    cassie_message_m_t_f_spawn: 
    # Makes a Chaos cassie message when the team spawns
    cassie_message_chaos_message: pitch_0.1 .g5 .g5 .g5 pitch_1 operation chaos force has entered the facility bell_end
    # set this to 0 to prevent the cassie announcement for chaos
    cassie_message_chaos_announce_chance: 100
    # Sets where this team spawns
    spawn_location: Escape
    # makes it where if this team is the latest spawn it will spawn the assigned escapees to this team if they are defined in this config
    escape_change:
    - Scientist
    - DClass
    color: green
    # the chance this team will spawn if its been selected
    chance: 100
  -
  # sets if the teams active
    active: true
    # Sets the team name MUST be lowercase
    name: aes
    # Defines subclasses of this team such as commander and rookie(tip: define commander first then Officer then rookie then ect...)
    subclasses:
    -
    # Sets the Subclasses name
      name: commander
      # sets the subclasses HP
      h_p: 100
      # sets what this class has
      inventory:
      - GunProject90
      - KeycardO5
      - Medkit
      - Medkit
      - WeaponManagerTablet
      - Radio
      - 6
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
        Nato762: 450
      # Sets what this role is supposed to look like
      model_role: ChaosInsurgency
      # sets the role name on the player to easily define who you are
      role_name: AES Commander
      # What message is displayed for the role itself
      role_message: You are to eliminate all scp subjects
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: 1
    -
    # Sets the Subclasses name
      name: firstofficer
      # sets the subclasses HP
      h_p: 100
      # sets what this class has
      inventory:
      - GunProject90
      - KeycardChaosInsurgency
      - Medkit
      - Medkit
      - WeaponManagerTablet
      - Radio
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
        Nato762: 450
      # Sets what this role is supposed to look like
      model_role: ChaosInsurgency
      # sets the role name on the player to easily define who you are
      role_name: AES First Officer
      # What message is displayed for the role itself
      role_message: You are to eliminate all scp subjects
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: 1
    -
    # Sets the Subclasses name
      name: officer
      # sets the subclasses HP
      h_p: 100
      # sets what this class has
      inventory:
      - GunMP7
      - KeycardZoneManager
      - Medkit
      - Medkit
      - WeaponManagerTablet
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
        Nato762: 450
      # Sets what this role is supposed to look like
      model_role: ChaosInsurgency
      # sets the role name on the player to easily define who you are
      role_name: AES Officer
      # What message is displayed for the role itself
      role_message: You are to eliminate all scp subjects
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: 2
    -
    # Sets the Subclasses name
      name: rookie
      # sets the subclasses HP
      h_p: 100
      # sets what this class has
      inventory:
      - GunMP7
      - KeycardZoneManager
      - Medkit
      - WeaponManagerTablet
      - WeaponManagerTablet
      # Define ammo so that this class has this ammo
      ammo:
        Nato9: 450
        Nato762: 450
      # Sets what this role is supposed to look like
      model_role: ChaosInsurgency
      # sets the role name on the player to easily define who you are
      role_name: AES rookie
      # What message is displayed for the role itself
      role_message: You are to eliminate all scp subjects
      # the amount of players that can be changed to this role when spawning setting this to -1 will make the rest of the players this Subteam
      num_of_allowed_players: -1
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - aes
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - mtf
    - opcf
    - goc
    - gru
    - chi
    - rsc
    - cdp
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - tta
    # Defines where this team can spawn by default it can spawn as both Chaos and NineTailedFox spawn locations
    spawn_types:
    - NineTailedFox
    # defines who wins at the end of the round if the Requirements perameter is accepted
    team_leaders: FacilityForces
    # Makes MTF cassie messages when this team spawns set it to nothing if you dont want an MTF cassie message
    cassie_message_m_t_f_spawn: cassie pitch_0.6 .g3 .g3 pitch_1 the arrival of the anomaly emergency squad {nato} {unit} has entered the facility. pitch_0.96 please escort2 all scpsubjects to surface zone. pitch_1 all scpsubjects need to be secured. please wait inside of your designated evacuation shelters until this emergency has been completed. there are {SCP} scpsubjects remaining.
    # Makes a Chaos cassie message when the team spawns
    cassie_message_chaos_message: ''
    # set this to 0 to prevent the cassie announcement for chaos
    cassie_message_chaos_announce_chance: 0
    # Sets where this team spawns
    spawn_location: SCP106
    # makes it where if this team is the latest spawn it will spawn the assigned escapees to this team if they are defined in this config
    escape_change:
    - Scientist
    - DClass
    color: red
    # the chance this team will spawn if its been selected
    chance: 50
  team_redefine:
  - active: true
    team: MTF
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - opcf
    - mtf
    - rsc
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - aes
    - goc
    - gru
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - rsc
    - chi
    - cdp
    - tta
    team_leaders: Anomalies
  - active: true
    team: CHI
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - chi
    - cdp
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - aes
    - goc
    - gru
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - rsc
    - mtf
    - cdp
    - tta
    - opcf
    team_leaders: Anomalies
  - active: true
    team: CDP
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - chi
    - cdp
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - aes
    - goc
    - gru
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - rsc
    - mtf
    - tta
    - opcf
    team_leaders: Anomalies
  - active: true
    team: RSC
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - mtf
    - rsc
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral:
    - aes
    - goc
    - gru
    - opcf
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - scp
    - rsc
    - mtf
    - cdp
    - tta
    - opcf
    team_leaders: Anomalies
  - active: true
    team: SCP
    # String values of teams MUST be lowercase to define correctly. you can define Exiled teams too! this defines who cant hurt this team
    friendlys:
    - scp
    # String values MUST be lowercase to define correctly. this defines teams that if alive when this team wins will make the round a draw
    neutral: []
    # String values of teams MUST be lowercase to define correctly and you can use Exiled teams too. this ends the round when none of these teams are in the round when this team is active(This is requirements due to the fact that You wouldnt want Scientist to win when MTF is here to try helping them escape. plus requirements are also for which teams are enemies and cant allow them to win)
    requirements:
    - rsc
    - mtf
    - cdp
    - tta
    - opcf
    - aes
    - goc
    - gru
    team_leaders: Anomalies
```
