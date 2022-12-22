namespace Namespace {
    
    using np = numpy;
    
    using math;
    
    using System.Collections.Generic;
    
    using System.Linq;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        //  This class returns the current positon of poppy in Vrep
        //     within a math way 
        public class pseudoCV
            : object {
            
            public pseudoCV(object poppy, object io, object name, object positionMatrix) {
                this.poppy = poppy;
                this.io = io;
                this.name = name;
                this.positionMatrix = positionMatrix;
            }
            
            //  Return the vector of camera foward direction 
            public virtual object @__headForwardDirection() {
                var angleNegativeY = this.poppy.head_z.present_position;
                var angleSurfaceXY = -this.poppy.head_y.present_position;
                angleNegativeY = angleNegativeY / 180 * 3.14159;
                angleSurfaceXY = angleSurfaceXY / 180 * 3.14159;
                var y = -np.cos(angleSurfaceXY) * np.cos(angleNegativeY);
                var x = np.cos(angleSurfaceXY) * np.sin(angleNegativeY);
                var z = np.sin(angleSurfaceXY);
                var forwardDire = new List<object> {
                    x,
                    y,
                    z
                };
                return forwardDire;
            }
            
            //  return the relativ positon(vector) of object to camera
            public virtual object @__objectRelPosition() {
                var objectPos = this.io.get_object_position(this.name);
                var positionCameraOri = new List<object> {
                    0,
                    -0.05,
                    1.06
                };
                var objectRelPos = (from i in xrange(3)
                    select (objectPos[i] - positionCameraOri[i])).ToList();
                return objectRelPos;
            }
            
            //  Judge if the object is in sight by calculating if
            //             the object is out of perspective 
            public virtual object @__canSeeJudge() {
                var orthognalBasis1 = this.@__headForwardDirection();
                var orthognalBasis2 = new List<object> {
                    orthognalBasis1[1],
                    -orthognalBasis1[0],
                    0
                };
                var normOrthBasis2 = np.linalg.norm(orthognalBasis2);
                orthognalBasis2 = (from i in xrange(3)
                    select (orthognalBasis2[i] / normOrthBasis2)).ToList();
                var orthognalBasis3 = np.cross(orthognalBasis2, orthognalBasis1);
                var objectRelPos = this.@__objectRelPosition();
                var objectProjectionOnOrthBasis1 = np.dot(objectRelPos, orthognalBasis1);
                if (objectProjectionOnOrthBasis1 < 0) {
                    return false;
                }
                var objectProjectionOnOrthBasis2 = np.dot(objectRelPos, orthognalBasis2);
                var objectProjectionOnOrthBasis3 = np.dot(objectRelPos, orthognalBasis3);
                var newCoordinate = new List<object> {
                    objectProjectionOnOrthBasis1,
                    objectProjectionOnOrthBasis2,
                    objectProjectionOnOrthBasis3
                };
                var tt = new List<object> {
                    1,
                    0,
                    0
                };
                var t = new List<object> {
                    objectProjectionOnOrthBasis1,
                    objectProjectionOnOrthBasis2,
                    0
                };
                var angle1 = np.arccos(np.dot(tt, t) / np.linalg.norm(t)) / 3.14159 * 180;
                if (abs(angle1) > 37) {
                    return false;
                }
                t = new List<object> {
                    objectProjectionOnOrthBasis1,
                    0,
                    objectProjectionOnOrthBasis3
                };
                var angle2 = np.arccos(np.dot(tt, t) / np.linalg.norm(t)) / 3.14159 * 180;
                if (abs(angle2) > 18.5) {
                    return false;
                }
                if (objectProjectionOnOrthBasis3 < 0 && angle2 > 0) {
                    angle2 = -angle2;
                }
                if (objectProjectionOnOrthBasis2 > 0 && angle1 > 0) {
                    angle1 = -angle1;
                }
                return Tuple.Create(angle1, angle2);
            }
            
            //  Return the position of centorid in state matrix
            //             if object out of perspective return () 
            public virtual object getPosition() {
                var angle = this.@__canSeeJudge();
                var _tup_1 = this.positionMatrix;
                var m = _tup_1.Item1;
                var n = _tup_1.Item2;
                if (!angle) {
                    return new List<object>();
                }
                var _tup_2 = angle;
                var angle1 = _tup_2.Item1;
                var angle2 = _tup_2.Item2;
                var x = math.floor(abs(np.sin(angle1 / 180.0 * 3.14159) / np.sin(37 / 180.0 * 3.14159) * (m + 1)));
                var y = math.floor(abs(np.sin(angle2 / 180.0 * 3.14159) / np.sin(18.5 / 180.0 * 3.14159) * (n + 1)));
                if (angle1 > 0) {
                    x = -x;
                }
                if (angle2 < 0) {
                    y = -y;
                }
                return (x, y);
            }
        }
    }
}
