using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kitware.VTK;

namespace DecimateFilterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            DecimateFilterTest1();
        }

        public static void DecimateFilterTest1()
        {
            vtkSTLReader pSTLReader = vtkSTLReader.New();
            pSTLReader.SetFileName("../../../../res/cow.stl");
            pSTLReader.Update();

            vtkDecimatePro decimatePro = vtkDecimatePro.New();
            decimatePro.SetInputConnection(pSTLReader.GetOutputPort());
            decimatePro.SetTargetReduction(0.9); // all mesh 10% shrink
            decimatePro.PreserveTopologyOn();
            decimatePro.Update();

            vtkQuadricClustering qClustering = vtkQuadricClustering.New();
            qClustering.SetInputConnection(pSTLReader.GetOutputPort());
            qClustering.SetNumberOfDivisions(10, 10, 10);
            qClustering.Update();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(decimatePro.GetOutputPort());
            //mapper.SetInputConnection(qClustering.GetOutputPort());

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);

            vtkRenderer renderer = vtkRenderer.New();
            renderer.AddActor(actor);
            renderer.SetBackground(.1, .2, .3);
            renderer.ResetCamera();

            vtkRenderWindow renderWin = vtkRenderWindow.New();
            renderWin.AddRenderer(renderer);

            vtkRenderWindowInteractor interactor = vtkRenderWindowInteractor.New();
            interactor.SetRenderWindow(renderWin);

            renderWin.Render();
            interactor.Start();
        }
    }
}
