namespace Namespace {
    
    using np = numpy;
    
    using math;
    
    using cv2;
    
    using System.Collections.Generic;
    
    using System.Linq;
    
    public static class Module {
        
        public static object @__author__ = "Zhiwei Han";
        
        //  This class returns the current positon by using color detection 
        public class CVAlgorithm
            : object {
            
            public CVAlgorithm() {
            }
            
            //  Take image
            public virtual object takeImage() {
                Func<object> get_image = () => {
                    var _tup_1 = camera.read();
                    var retval = _tup_1.Item1;
                    var im = _tup_1.Item2;
                    return im;
                };
                var camera = cv2.VideoCapture(0);
                var ramp_frames = 3;
                foreach (var i in xrange(ramp_frames)) {
                    var temp = get_image();
                }
                cv2.imshow("dd", temp);
                cv2.waitKey(0);
                return temp;
            }
            
            //  Get binary mask of the image 
            public virtual object @__getMask(object image) {
                var hsv = cv2.cvtColor(image, cv2.COLOR_BGR2HSV);
                var lower_blue = np.array(new List<object> {
                    115,
                    50,
                    50
                });
                var upper_blue = np.array(new List<object> {
                    125,
                    255,
                    255
                });
                var lower_red = np.array(new List<object> {
                    -3,
                    50,
                    50
                });
                var upper_red = np.array(new List<object> {
                    3,
                    255,
                    255
                });
                var lower_green = np.array(new List<object> {
                    50,
                    50,
                    50
                });
                var upper_green = np.array(new List<object> {
                    70,
                    255,
                    255
                });
                var maskBlue = cv2.inRange(hsv, lower_blue, upper_blue);
                maskBlue = cv2.blur(maskBlue, (20, 20));
                var _tup_1 = cv2.threshold(maskBlue, 127, 255, cv2.THRESH_BINARY);
                var ret = _tup_1.Item1;
                maskBlue = _tup_1.Item2;
                var maskRed = cv2.inRange(hsv, lower_red, upper_red);
                maskRed = cv2.blur(maskRed, (20, 20));
                var _tup_2 = cv2.threshold(maskRed, 127, 255, cv2.THRESH_BINARY);
                ret = _tup_2.Item1;
                maskRed = _tup_2.Item2;
                return Tuple.Create(maskRed, maskBlue);
            }
            
            //  detect the color (red or blue)
            public virtual object @__getColour(object maskRed, object maskBlue) {
                object color;
                var countRed = (from row in maskRed
                    select (from i in row
                        where i
                        select 1).Sum()).Sum();
                var countBlue = (from row in maskBlue
                    select (from i in row
                        where i
                        select 1).Sum()).Sum();
                if (countRed > 1000 || countBlue > 1000) {
                    if (countBlue > countRed) {
                        color = "Blue";
                    } else {
                        color = "Red";
                    }
                    return color;
                }
                return false;
            }
            
            //  Calculate the centroid of object
            public virtual object @__getCentroid(object mask) {
                var _tup_1 = mask.nonzero();
                var x = _tup_1.Item1;
                var y = _tup_1.Item2;
                x = np.int0(x.mean());
                y = np.int0(y.mean());
                var centroid = (x, y);
                return centroid;
            }
            
            //  Return the centroid position of object 
            public virtual object getPosition() {
                object centroid;
                var image = this.takeImage();
                // print m, n
                var _tup_1 = this.@__getMask(image);
                var maskRed = _tup_1.Item1;
                var maskBlue = _tup_1.Item2;
                var color = this.@__getColour(maskRed, maskBlue);
                if (!color) {
                    return ValueTuple.Create("<Empty>");
                }
                if (color == "Blue") {
                    centroid = this.@__getCentroid(maskBlue);
                } else {
                    centroid = this.@__getCentroid(maskRed);
                }
                var c = (centroid.ToList()[1], centroid.ToList()[0]);
                cv2.drawContours(image, new List<object> {
                    np.int0(new List<object> {
                        c,
                        c,
                        c,
                        c
                    })
                }, 0, (255, 0, 0), 7);
                cv2.imshow("dd", image);
                cv2.waitKey(0);
                return (centroid, maskRed.shape);
            }
        }
        
        public static object cv = CVAlgorithm();
    }
}
