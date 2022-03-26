// Copyright Epic Games, Inc. All Rights Reserved.

#include "BilardoGameMode.h"
#include "BilardoHUD.h"
#include "BilardoCharacter.h"
#include "UObject/ConstructorHelpers.h"

ABilardoGameMode::ABilardoGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPersonCPP/Blueprints/FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

	// use our custom HUD class
	HUDClass = ABilardoHUD::StaticClass();
}
