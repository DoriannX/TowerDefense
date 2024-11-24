using System.Linq;
using SerializedProperties;
using UnityEngine;

public class CharacterGroundChecker
{
    private readonly RaycastHit[] _hitsInfo;
    private readonly CharacterBodyProperties _characterBodyProperties;
    private readonly CharacterGroundProperties _characterGroundProperties;


    public CharacterGroundChecker(ref CharacterBodyProperties characterBodyProperties,
        ref CharacterGroundProperties characterGroundProperties)
    {
        _characterBodyProperties = characterBodyProperties;
        _characterGroundProperties = characterGroundProperties;
        _hitsInfo = new RaycastHit[10];
    }

    public bool IsGrounded()
    {
        int hitsCount = Physics.SphereCastNonAlloc(
            _characterBodyProperties.Transform.position,
            _characterBodyProperties.Extents.z - _characterBodyProperties.SkinWidth,
            Vector3.down,
            _hitsInfo,
            _characterGroundProperties.RaycastDistance,
            _characterGroundProperties.GroundLayer);
            
        bool isGrounded = hitsCount > 0 && _hitsInfo.Any(hitInfo => hitInfo.collider is not null);
            
        return isGrounded;
    }
}