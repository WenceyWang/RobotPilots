#pragma once

#include "InterOp.hpp"

class CVCamera :ICamera
{
public:

	shared_ptr< cv::VideoCapture> VideoCapture;

	bool GetIsOpened() override
	{
		return VideoCapture == nullptr;
	}

	CVCamera(int index)
	{
		VideoCapture = make_shared<cv::VideoCapture>(index);
	}

	shared_ptr<cv::Mat> Read() override
	{
		auto frame = make_shared<cv::Mat>();
		VideoCapture->read(*frame);
		return frame;
	}

};

class CalibratedCamera:ICalibratedCamera
{
public:

	///Get if this Camera Calibrated
	bool GetIsCalibrated()
	{
		return CameraMatrix != nullptr && DistortionCoefficients != nullptr;
	}

	///Get if this camera opened
	bool GetIsOpened()
	{
		return Camera != nullptr;
	}

	shared_ptr<ICamera> Camera;

	shared_ptr<cv::Mat> CameraMatrix;

	shared_ptr<cv::Mat> DistortionCoefficients;

	void CalibrateFromImages(vector<cv::Mat>& images, ChessBoard& chessBoard)
	{
		vector<vector<cv::Point3f>> objectPointsOfFrames;

		vector<cv::Point3f> objectPoints;
		chessBoard.GetObjectPoints(objectPoints);

		vector<vector<cv::Point2f>> imagePointsOfFrames;

		auto imageSize = images.front().size();

		for each (auto image in images)
		{
			cv::Mat imageGrayscale;
			cvtColor(image, imageGrayscale, CV_BGR2GRAY);

			vector<cv::Point2f> chessBoardCorners;
			cv::findChessboardCorners(imageGrayscale, cv::Size(chessBoard.Size.width, chessBoard.Size.height), chessBoardCorners);

			imagePointsOfFrames.push_back(chessBoardCorners);

			objectPointsOfFrames.push_back(objectPoints);

		}

		cv::Mat R, T;

		cv::calibrateCamera(objectPointsOfFrames, imagePointsOfFrames, imageSize, *CameraMatrix, *DistortionCoefficients, R, T);

	}

	CalibratedCamera(shared_ptr<ICamera> camera)
	{
		Camera = camera;
		CameraMatrix = nullptr;
		DistortionCoefficients = nullptr;
	}

	///Read Original Frame
	shared_ptr<cv::Mat> ReadOriginal() override
	{
		if (GetIsOpened())
		{
			return Camera->Read();
		}
		else
		{
			return false;
		}
	}

	///if Calibrated ,read a Calibrated Frame, or Original.
	shared_ptr<cv::Mat> Read() override
	{
		if (GetIsOpened())
		{
			auto frame = ReadOriginal();
			if (GetIsCalibrated())
			{
				
			}
			else
			{
				return;
			}
		}
		else
		{
			return nullptr;
		}
	}

	~CalibratedCamera()
	{

	}

};

template<class TCamera>
class BinocularCamera :IBinocularCamera
{
public:

	shared_ptr<CalibratedCamera<TCamera>> LeftCamera;

	shared_ptr<CalibratedCamera<TCamera>> RightCamera;


	bool Read(cv::Mat& left, cv::Mat& right, cv::Mat& depth)
	{
		cv::Mat leftOriginal, rightOriginal, disparity;

		if (!LeftCamera->ReadOriginal(leftOriginal))return false;
		if (!RightCamera->ReadOriginal(rightOriginal))return false;

		cv::remap(leftOriginal, left, *mapX1, *mapY1, cv::InterpolationFlags::INTER_CUBIC);
		cv::remap(rightOriginal, right, *mapX2, *mapY2, cv::InterpolationFlags::INTER_CUBIC);

		Stereo->compute(left, right, disparity);

		cv::reprojectImageTo3D(disparity, depth, *Q);

	}

	///Open this Camera
	void Open(int leftCameraID, int rightCameraID, cv::Size frameSize)
	{
		auto leftCameraSource = new cv::VideoCapture(leftCameraID);
		leftCameraSource->set(cv::CAP_PROP_FRAME_HEIGHT, frameSize.height);
		leftCameraSource->set(cv::CAP_PROP_FRAME_WIDTH, frameSize.width);

		auto rightCameraSource = new cv::VideoCapture(rightCameraID);
		rightCameraSource->set(cv::CAP_PROP_FRAME_HEIGHT, frameSize.height);
		rightCameraSource->set(cv::CAP_PROP_FRAME_WIDTH, frameSize.width);

		LeftCamera = new CalibratedCamera(leftCameraSource);
		RightCamera = new CalibratedCamera(rightCameraSource);
	}

	///Calibrate this camera from vector of frames
	void StereoCalibrateFromImages(vector<tuple<cv::Mat, cv::Mat>> images, ChessBoard& chessBoard)
	{
		auto a = cv::StereoSGBM::create();

		//vector of (vector of (every point on chess board) of every frame)
		vector<vector<cv::Point3f>> objectPointsOfFrames;

		vector<cv::Point3f> objectPoints;
		chessBoard.GetObjectPoints(objectPoints);

		//vector of (vector of (every point on chess board) of every frame)
		vector<vector<cv::Point2f>> LeftImagePointsOfFrames;
		vector<vector<cv::Point2f>> RightImagePointsOfFrames;

		cv::Size imageSize;

		for each (tuple<cv::Mat, cv::Mat> imagePair in images)
		{
			cv::Mat leftImage, rightImage;

			tie(leftImage, rightImage) = imagePair;

			if (leftImage.size() != rightImage.size())
			{
				continue;
			}

			imageSize = leftImage.size();

			cv::Mat leftImageGrayscale;
			cvtColor(leftImage, leftImageGrayscale, CV_BGR2GRAY);

			cv::Mat rightImageGrayscale;
			cvtColor(rightImage, rightImageGrayscale, CV_BGR2GRAY);

			vector<cv::Point2f> leftChessboardCorners;
			cv::findChessboardCorners(leftImageGrayscale, cv::Size(chessBoard.Size.width, chessBoard.Size.height), leftChessboardCorners);

			vector<cv::Point2f> rightChessboardCorners;
			cv::findChessboardCorners(rightImageGrayscale, cv::Size(chessBoard.Size.width, chessBoard.Size.height), rightChessboardCorners);

			if (leftChessboardCorners.size() == rightChessboardCorners.size())
			{
				LeftImagePointsOfFrames.push_back(leftChessboardCorners);
				RightImagePointsOfFrames.push_back(rightChessboardCorners);
				objectPointsOfFrames.push_back(objectPoints);
			}
		}

		cv::Mat	LeftCameraCameraMatrix;
		cv::Mat	LeftCameraDistortionCoefficients;
		cv::Mat	RightCameraCameraMatrix;
		cv::Mat	RightCameraDistortionCoefficients;

		cv::Mat R, T, E, F;

		cv::stereoCalibrate(
			objectPointsOfFrames,
			LeftImagePointsOfFrames,
			RightImagePointsOfFrames,
			LeftCameraCameraMatrix,
			LeftCameraDistortionCoefficients,
			RightCameraCameraMatrix,
			RightCameraDistortionCoefficients,
			imageSize,
			R, T, E, F, 0);

		LeftCamera->CameraMatrix = new cv::Mat(LeftCameraCameraMatrix);
		LeftCamera->DistortionCoefficients = new cv::Mat(LeftCameraDistortionCoefficients);

		RightCamera->CameraMatrix = new cv::Mat(RightCameraCameraMatrix);
		RightCamera->DistortionCoefficients = new cv::Mat(RightCameraDistortionCoefficients);

		cv::Mat R1, R2, P1, P2, Q;

		cv::stereoRectify(
			LeftCameraCameraMatrix,
			LeftCameraDistortionCoefficients,
			RightCameraCameraMatrix,
			RightCameraDistortionCoefficients,
			imageSize,
			R, T, R1, R2, P1, P2, Q);

		cv::initUndistortRectifyMap(
			LeftCameraCameraMatrix,
			LeftCameraDistortionCoefficients,
			R1, P1,
			imageSize,
			CV_16SC2,
			*mapX1, *mapY1);

		cv::initUndistortRectifyMap(
			RightCameraCameraMatrix,
			RightCameraDistortionCoefficients,
			R2, P2,
			imageSize,
			CV_16SC2,
			*mapX2, *mapY2);

	}

	shared_ptr< cv::Mat> mapX1 = new cv::Mat();
	shared_ptr< cv::Mat>mapY1 = new cv::Mat(); 
	shared_ptr< cv::Mat>mapX2 = new cv::Mat();
	shared_ptr< cv::Mat>mapY2 = new cv::Mat(); 
	shared_ptr< cv::Mat> Q = new cv::Mat();

	cv::StereoSGBM *Stereo = (cv::StereoSGBM::create(0, 256, 7, 8 * 3 * 3 * 3, 32 * 3 * 3 * 3, -1, 64, 10, 150, 2, 2));

	~BinocularCamera()
	{
		if (LeftCamera != nullptr)
		{
			delete LeftCamera;
		}
		if (RightCamera != nullptr)
		{
			delete RightCamera;
		}
		delete mapX1, mapY1, mapX2, mapY2, Q;
		delete Stereo;
	}


};

