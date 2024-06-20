using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { isFree, isInMenu, isBuilding, isBuildingWalls };
public enum BuildingState { withPhysics, withOffset, withTrigger /* Cambiar el nombre ya que no se usa Trigger*/};
