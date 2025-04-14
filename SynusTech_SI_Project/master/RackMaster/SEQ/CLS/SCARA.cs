using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RackMaster.SEQ.CLS {
    public class SCARA {
        private double RZ_LENGTH;
        private double RX_LENGTH;
        private double RY_LENGTH;

        private double RZ_InitialLength;
        private double offset_angle;
        private double alpha;
        double MaximumLength;

        public SCARA() {
            RX_LENGTH = 220;
            RY_LENGTH = 220;
            RZ_LENGTH = 420;
            alpha = 1;
            RZ_InitialLength = 225;
            offset_angle = Math.Acos((RZ_InitialLength / 2) / RX_LENGTH);
            MaximumLength = RX_LENGTH + RY_LENGTH + RZ_InitialLength;
        }

        public SCARA(double RX, double RY, double RZ) {
            RX_LENGTH = RX;
            RY_LENGTH = RY;
            RZ_LENGTH = RZ;
            alpha = 1;
            RZ_InitialLength = 225;
            offset_angle = Math.Acos((RZ_InitialLength / 2) / RX_LENGTH);
            MaximumLength = RX_LENGTH + RY_LENGTH + RZ_InitialLength;
        }

        public double GetAngle(double Target) {
            double ThetaInDegree = 0;
            double ThetaInRadian = 0;
            if (!IsTargetBiggerThanMaximumLength(Target)) {
                if (IsTargetBiggerThanIntialLength(Target)) {
                    ThetaInRadian = Math.PI - Math.Acos((Target - RZ_InitialLength) / 2 / RX_LENGTH) - offset_angle;
                }

                else {
                    ThetaInRadian = Math.Acos((RZ_InitialLength - Target) / 2 / RX_LENGTH) - offset_angle;
                }

            }
            else {
                throw new ArithmeticException();
            }
            ThetaInRadian *= alpha;
            ThetaInDegree = Math.Round(RadianToDegree(ThetaInRadian), 6);
            return ThetaInDegree;
        }

        public double GetPosition(double currentAngle) {
            double currentPosition = 0;
            double actualAngleInDegree = currentAngle + RadianToDegree(offset_angle);
            double actualAngleInRadian = DegreeToRadian(actualAngleInDegree);

            currentPosition = Math.Cos(Math.PI - actualAngleInRadian) * RX_LENGTH * 2;
            currentPosition += RZ_InitialLength;
            currentPosition = Math.Round(currentPosition, 6);
            return currentPosition;
        }

        private double RadianToDegree(double radian) {
            double degree = 0;

            degree = radian * (180 / Math.PI);
            return degree;
        }

        private double DegreeToRadian(double degree) {
            double radian = 0;

            radian = degree * (Math.PI / 180);
            return radian;
        }

        public double GetMaximumLength() {
            return MaximumLength;
        }

        private bool IsTargetBiggerThanIntialLength(double Target) {
            bool ret = false;

            ret = (Target >= RZ_InitialLength) ? true : false;
            return ret;
        }

        private bool IsTargetBiggerThanMaximumLength(double Target) {
            bool ret = false;

            ret = (Target > MaximumLength) ? true : false;
            return ret;
        }

        public void SetInitialLength(double InitialLength) {
            RZ_InitialLength = InitialLength;
            offset_angle = Math.Acos((RZ_InitialLength / 2) / RX_LENGTH);
        }

        public double GetInitialLength() {
            return RZ_InitialLength;
        }

        public void SetOffsetAngle(double offsetAngle) {
            offsetAngle = DegreeToRadian(offsetAngle);
            offset_angle = offsetAngle;
        }

        public void SetXLength(double Length) {
            RX_LENGTH = Length;
        }

        public void SetYLength(double Length) {
            RY_LENGTH = Length;
        }

        public void SetZLength(double Length) {
            RZ_LENGTH = Length;
        }

        public void SetAlpha(double Alpha) {
            alpha = Alpha;
        }

        public double GetAlpha() {
            return alpha;
        }

        public void SetMaxmimumLength(double maximumLength) {
            MaximumLength = maximumLength;
        }
    }
}
