#pragma once

#include<Stdafx.h>

class Object
{
public:

	~Object()
	{

	}

	Object()
	{

	}

};

class ChessBoard :Object
{
public:

	cv::Size Size;

	float SqureSize;

	ChessBoard() {}

	ChessBoard(cv::Size size, float squreSize)
	{
		Size = size;
		SqureSize = squreSize;
	}


	/** @brief Output every cross point to a vector

	*/
	void GetObjectPoints(vector<cv::Point3f>& objectPoints)
	{
		for (int y = 0; y < Size.height; y++)
		{
			for (int x = 0; x < Size.width; x++)
			{
				objectPoints.push_back(cv::Point3f(x*Size.width * SqureSize, y*Size.height * SqureSize, 0));
			}
		}
	}

};

class ICamera :public Object
{
public:

	virtual bool GetIsOpened() = 0;

	virtual shared_ptr<cv::Mat> Read() = 0;

};

class ICalibratedCamera :public ICamera
{
public:

	virtual bool GetIsCalibrated() = 0;

	virtual shared_ptr<cv::Mat> ReadOriginal() = 0;

};

class IBinocularCamera :Object
{



};


class ITripodHead :public Object
{
	virtual cv::Point GetCurrentPosition() = 0;


};

class IRecognizer : public Object
{
public:
	virtual vector<cv::Point> GetTarget() = 0;

};

class I3DRebuilder : public Object
{
public:

	virtual vector<cv::Point3f> Transform(vector<cv::Point> points) = 0;
};

enum ControllerStatus
{
	Running,
	Stopped,

};

class IPredictor : public Object
{
public:

	virtual cv::Point3f Predict(vector<tuple<int, cv::Point3f>> positions, int targetTime) = 0;

};

class IController : public Object
{

public:

	//virtual 

};

class Application : public Object
{
public:

	int Run()
	{

		return 0;
	}

};
